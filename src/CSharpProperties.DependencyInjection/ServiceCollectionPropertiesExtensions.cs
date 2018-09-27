using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CSharpProperties.DependencyInjection.Annotations;
using CSharpProperties.DependencyInjection.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MosaicoSolutions.CSharpProperties;

namespace CSharpProperties.DependencyInjection
{
    public static class ServiceCollectionPropertiesExtensions
    {
        public static IServiceCollection AddPropertiesFiles(this IServiceCollection services)
        {
            var allTypes = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(a => a.GetTypes()
                                                      .Where(t => t.IsClass &&
                                                                  t.IsDefined(typeof(PropertiesFileAttribute), false)));

            foreach (var type in allTypes)
                services.AddProperties(type);

            return services;
        }

        private static void AddProperties(this IServiceCollection services, Type type)
        {
            if (type.HasDefaultConstructor())
                services.ResolveProperties(type, () => Activator.CreateInstance(type));
            else
                services.ResolveProperties(type);
        }

        private static void ResolveProperties(this IServiceCollection services, Type type, Func<object> newInstance)
        {
            services.AddTransient(type, serviceProvider => 
            {
                var instance = newInstance();
                LoadProperties(instance);
                return instance;
            });
        }

        private static void ResolveProperties(this IServiceCollection services, Type type)
        {
            services.AddTransient(type, serviceProvider =>
            {
                object instance = null;

                foreach (var constructor in type.GetConstructors())
                {
                    var constructorParameters = constructor.GetParameters().Where(p => !p.IsOptional).ToArray();

                    var parameters = new object[constructorParameters.Count()];

                    for (var i = 0; i < constructorParameters.Length; i++)
                    {
                        var service = serviceProvider.GetService(constructorParameters[i].ParameterType);

                        if (service is null)
                            break;

                        parameters[i] = service;
                    }

                    if (parameters.Any(p => p is null))
                        continue;

                    instance = Activator.CreateInstance(type, parameters);

                    if (instance != null)
                        break;
                }

                if (instance is null)
                    throw new InvalidOperationException($"Cannot resolve service of type {type.Name}");

                LoadProperties(instance);
                return instance;
            });
        }

        private static void LoadProperties(object instance)
        {
            var propertiesFileAttribute = instance.GetType()
                                                  .GetCustomAttributes(false)
                                                  .Where(a => a is PropertiesFileAttribute)
                                                  .Cast<PropertiesFileAttribute>()
                                                  .FirstOrDefault();

            if (!File.Exists(propertiesFileAttribute.Path))
                throw new FileNotFoundException("Properties File not found.", propertiesFileAttribute.Path);

            var properties = LoadPropertiesFromFile(propertiesFileAttribute.Path);

            if (!properties.Any())
                return;

            var propertiesKeys = properties.Keys;

            foreach (var property in instance.GetType().GetProperties().Where(p => p.CanWrite &&
                                                                             (p.PropertyType.IsPrimitive || p.PropertyType == typeof(string)) ||
                                                                             (p.PropertyType.IsNullableOfPrimitiveType())))
            {
                if (!propertiesKeys.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase))
                    continue;

                var key = propertiesKeys.FirstOrDefault(k => k.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                if (key is null)
                    continue;

                var value = properties[key];
                SetProperty(instance, property, value);
            }
        }

        private static IProperties LoadPropertiesFromFile(string path)
        {
            if (path.EndsWith(".json"))
                return Properties.LoadFromJson(path);

            if (path.EndsWith(".xml"))
                return Properties.LoadFromXml(path);

            if (path.EndsWith(".csv"))
                return Properties.LoadFromCsv(path);

            return Properties.Load(path);
        }

        private static void SetProperty(object instance, PropertyInfo property, string value)
        {
            var propertySetter = PropertySetter.FromType(property.PropertyType);

            propertySetter(instance, property, value);
        }
    }
}