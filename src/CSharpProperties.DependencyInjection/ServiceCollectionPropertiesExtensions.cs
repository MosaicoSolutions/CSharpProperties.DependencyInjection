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

namespace CSharpProperties.DependencyInjection
{
    public static class ServiceCollectionPropertiesExtensions
    {
        private static string BaseDirectory { get; set; }

        public static IServiceCollection AddPropertiesFiles(this IServiceCollection services, string baseDirectory = null)
        {
            BaseDirectory = baseDirectory;

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

                var propertiesFileAttribute = instance.GetType()
                                                      .GetCustomAttributes(false)
                                                      .OfType<PropertiesFileAttribute>()
                                                      .FirstOrDefault();

                var finalPath = $@"{BaseDirectory ?? string.Empty}\{propertiesFileAttribute.Path}";

                if (!File.Exists(finalPath))
                    throw new FileNotFoundException("Properties File not found.", finalPath);

                var properties = LoadPropertiesFromFile(finalPath);

                if (!properties.Any())
                    return instance;

                LoadProperties(instance, properties);
                LoadFields(instance, properties);
                return instance;
            });
        }

        private static void LoadProperties(object instance, IProperties properties)
        {
            var propertiesKeys = properties.Keys;

            foreach (var property in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                                       .Where(p => p.CanWrite &&
                                                                  (p.PropertyType.IsPrimitive || p.PropertyType == typeof(string)) ||
                                                                   p.PropertyType.IsNullableOfAnyPrimitiveType()))
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
                                      .OfType<PropertiesKeyAttribute>()
                                      .Select(a => a.Key)
                                      .FirstOrDefault()
                            : property.Name;

            var key = propertiesKeys.FirstOrDefault(k => k.Equals(keyName, StringComparison.InvariantCultureIgnoreCase));

            return key;
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
                if (string.IsNullOrEmpty(value))
                {
                    property.SetValue(target, null);
                    return;
                }

                propertyType = new NullableConverter(property.PropertyType).UnderlyingType;
            }

            var convertedValue = Convert.ChangeType(value, propertyType);
            property.SetValue(target, convertedValue);
        }

        private static void LoadFields(object instance, IProperties properties)
        {
            var propertiesKeys = properties.Keys;

            foreach (var field in instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                                    .Where(f => (f.FieldType.IsPrimitive || f.FieldType == typeof(string)) || f.FieldType.IsNullableOfAnyPrimitiveType()))
            {
                var key = ExtractKey(field, propertiesKeys);
                
                if (key is null)
                    continue;

                var value = properties[key];
                SetFieldValueFromString(instance, field, value);
            }
        }

        private static string ExtractKey(FieldInfo field, IEnumerable<string> propertiesKeys)
        {
            var keyName = field.IsDefined(typeof(PropertiesKeyAttribute), false)
                            ? field.GetCustomAttributes(false)
                                   .OfType<PropertiesKeyAttribute>()
                                   .Select(a => a.Key)
                                   .FirstOrDefault()
                            : field.Name;

            var key = propertiesKeys.FirstOrDefault(k => k.Equals(keyName, StringComparison.InvariantCultureIgnoreCase));

            return key;
        }

        private static void SetFieldValueFromString(object target, FieldInfo field, string value)
        {
            if  (field.FieldType == typeof(string))
            {
                field.SetValue(target, value);
                return;
            }

            var fieldType = field.FieldType;

            if (fieldType.IsNullable())
            {
                if (string.IsNullOrEmpty(value))
                {
                    field.SetValue(target, null);
                    return;
                }

                fieldType = new NullableConverter(field.FieldType).UnderlyingType;
            }

            var convertedValue = Convert.ChangeType(value, fieldType);
            field.SetValue(target, convertedValue);
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
    }
}