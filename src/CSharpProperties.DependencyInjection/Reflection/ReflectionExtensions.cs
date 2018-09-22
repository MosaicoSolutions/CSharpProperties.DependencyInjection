using System;
using System.Linq;
using System.Reflection;

namespace CSharpProperties.DependencyInjection.Reflection
{
    public static class ReflectionExtensions
    {
        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetConstructors().All(c => !c.GetParameters().Any() || c.GetParameters().All(p => p.IsOptional));
        }

        public static bool IsNullableOfPrimitiveType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                   type.GetGenericArguments().Any(t => t.IsValueType && t.IsPrimitive);
        }
    }
}