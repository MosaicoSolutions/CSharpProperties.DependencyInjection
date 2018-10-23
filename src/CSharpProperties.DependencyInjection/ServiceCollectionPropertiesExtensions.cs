using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CSharpProperties.DependencyInjection.Annotations;
using CSharpProperties.DependencyInjection.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MosaicoSolutions.CSharpProperties;
using static System.String;

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
                                                                  t.IsDefined(typeof(PropertiesFileAttribute), false) &&
                                                                  t.HasDefaultOrOptionalConstructor()));

            foreach (var type in allTypes)
                services.ResolveProperties(type, () => Activator.CreateInstance(type));

            return services;
        }

        private static void ResolveProperties(this IServiceCollection services, Type type, Func<object> newInstance)
        {
            services.AddScoped(type, serviceProvider => 
            {
                var instance = newInstance();
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
                                                                             (p.PropertyType.IsNullableOfAnyPrimitiveType())))
            {
                var key = ExtractKey(property, propertiesKeys);
                
                if (key is null)
                    continue;

                var value = properties[key];
                SetPropertyValueFromString(instance, property, value);
            }
        }

        private static string ExtractKey(PropertyInfo property, IEnumerable<string> propertiesKeys)
        {
            var keyName = property.IsDefined(typeof(PropertiesKeyAttribute), false)
                            ? property.GetCustomAttributes(false)
                                      .Where(a => a is PropertiesKeyAttribute)
                                      .Cast<PropertiesKeyAttribute>()
                                      .Select(a => a.Key)
                                      .FirstOrDefault()
                            : property.Name;

            var key = propertiesKeys.FirstOrDefault(k => k.Equals(keyName, StringComparison.InvariantCultureIgnoreCase));

            return key;
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
    
        private static void SetPropertyValueFromString(object target, PropertyInfo property, string value)
        {
            if  (property.PropertyType == typeof(string))
            {
                property.SetValue(target, value);
                return;
            }

            var propertyType = property.PropertyType;

            if (propertyType.IsNullable())
            {
                if (IsNullOrEmpty(value))
                {
                    property.SetValue(target, null, null);
                    return;
                }

                propertyType = new NullableConverter(property.PropertyType).UnderlyingType;
            }

            var convertedValue = Convert.ChangeType(value, propertyType);
            property.SetValue(target, convertedValue, null);
        }
    }
}