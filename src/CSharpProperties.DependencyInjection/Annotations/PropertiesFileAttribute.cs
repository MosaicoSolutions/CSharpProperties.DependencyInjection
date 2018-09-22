using System;

namespace CSharpProperties.DependencyInjection.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PropertiesFileAttribute : Attribute
    {
        public string Path { get; set; }
    }
}