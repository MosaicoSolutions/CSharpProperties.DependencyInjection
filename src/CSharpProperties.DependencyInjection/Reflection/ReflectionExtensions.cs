using System;
using System.Linq;
using System.Reflection;

namespace CSharpProperties.DependencyInjection.Reflection
{
    public static class ReflectionExtensions
    {
        public static bool HasDefaultOrOptionalConstructor(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetConstructors().All(c => !c.GetParameters().Any() || c.GetParameters().All(p => p.IsOptional));
        }

        public static bool IsNullable(this Type type)
        {
            return type == null
                    ? throw new ArgumentNullException(nameof(type))
                    : type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullableOfAnyPrimitiveType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                   type.GetGenericArguments().Any(t => t.IsValueType && t.IsPrimitive);
        }
    }
}