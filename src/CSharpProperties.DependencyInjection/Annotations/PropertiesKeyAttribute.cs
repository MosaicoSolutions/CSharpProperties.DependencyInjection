using System;

namespace CSharpProperties.DependencyInjection.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class PropertiesKeyAttribute : Attribute
    {
        public string Key { get; set; }
    }
}