using System;
using System.Linq;
using System.Reflection;
using CSharpProperties.DependencyInjection.Reflection;

namespace CSharpProperties.DependencyInjection
{
    internal delegate void PropertySetterStrategy(object instance, PropertyInfo property, string value);

    internal class PropertySetter
    {
        internal static PropertySetterStrategy FromType(Type type)
        {
            if (type == typeof(string))
                return StringPropertySetter;

            if (type == typeof(bool))
                return BoolPropertySetter;
            
            if (type == typeof(byte))
                return BytePropertySetter;

            if (type == typeof(sbyte))
                return SBytePropertySetter;

            if (type == typeof(char))
                return CharPropertySetter;

            if (type == typeof(decimal))
                return DecimalPropertySetter;

            if (type == typeof(double))
                return DoublePropertySetter;

            if (type == typeof(float))
                return FloatPropertySetter;
            
            if (type == typeof(int))
                return IntPropertySetter;

            if (type == typeof(uint))
                return UintPropertySetter;

            if (type == typeof(long))
                return LongPropertySetter;

            if (type == typeof(ulong))
                return ULongPropertySetter;

            if (type == typeof(short))
                return ShortPropertySetter;    

            if (type == typeof(ushort))
                return UshortPropertySetter;

            throw new InvalidOperationException($"There is no PropertySetterStrategy for type [{type.Name}].");
        }

        internal static PropertySetterStrategy StringPropertySetter
            => (instance, property, value) => 
            {
                property.SetValue(instance, value);
            };

        internal static PropertySetterStrategy BoolPropertySetter
            => (instance, property, value) => 
            {
                if (bool.TryParse(value, out bool result))
                {
                    property.SetValue(instance, result);
                } 
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(bool));
                }
            };

        internal static PropertySetterStrategy BytePropertySetter
            => (instance, property, value) =>
            {
                if (byte.TryParse(value, out byte result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(byte));
                }
            };

        internal static PropertySetterStrategy SBytePropertySetter
            => (instance, property, value) =>
            {
                if (sbyte.TryParse(value, out sbyte result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(sbyte));
                }
            };

        internal static PropertySetterStrategy CharPropertySetter
            => (instance, property, value) =>
            {
                if (char.TryParse(value, out char result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(char));
                }
            };

        internal static PropertySetterStrategy DecimalPropertySetter
            => (instance, property, value) =>
            {
                if (decimal.TryParse(value, out decimal result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(decimal));
                }
            };

        internal static PropertySetterStrategy DoublePropertySetter
            => (instance, property, value) =>
            {
                if (double.TryParse(value, out double result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(double));
                }
            };    

        internal static PropertySetterStrategy FloatPropertySetter
            => (instance, property, value) =>
            {
                if (float.TryParse(value, out float result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(float));
                }
            };  

        internal static PropertySetterStrategy IntPropertySetter
            => (instance, property, value) =>
            {
                if (int.TryParse(value, out int result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(int));
                }
            }; 

        internal static PropertySetterStrategy UintPropertySetter
            => (instance, property, value) =>
            {
                if (uint.TryParse(value, out uint result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(uint));
                }
            }; 

        internal static PropertySetterStrategy LongPropertySetter
            => (instance, property, value) =>
            {
                if (long.TryParse(value, out long result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(long));
                }
            };  

        internal static PropertySetterStrategy ULongPropertySetter
            => (instance, property, value) =>
            {
                if (ulong.TryParse(value, out ulong result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(ulong));
                }
            };         

        internal static PropertySetterStrategy ShortPropertySetter
            => (instance, property, value) =>
            {
                if (short.TryParse(value, out short result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(short));
                }
            };

        internal static PropertySetterStrategy UshortPropertySetter
            => (instance, property, value) =>
            {
                if (ushort.TryParse(value, out ushort result))
                {
                    property.SetValue(instance, result);
                }
                else if (property.PropertyType.IsNullableOfAnyPrimitiveType())
                {
                    property.SetValue(instance, null);
                } 
                else 
                {
                    property.SetValue(instance, default(ushort));
                }
            };                        
    }
}