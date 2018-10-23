using CSharpProperties.DependencyInjection.Annotations;

namespace CSharpProperties.DependencyInjection.Test.ViewModels
{
    [PropertiesFile(Path = @".\properties-files\properties-key-attribute.json")]
    public class PropertiesKeyAttributeViewModel
    {
        [PropertiesKey(Key = "database")]
        public string DatabaseName { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}