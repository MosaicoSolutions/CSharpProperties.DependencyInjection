using CSharpProperties.DependencyInjection.Annotations;

namespace CSharpProperties.DependencyInjection.Test.ViewModels
{
    [PropertiesFile(Path = @".\properties-files\using-fields.xml")]
    public class UsingFieldsViewModel
    {
        [PropertiesKey(Key = "app-name")]
        private string appName;
        [PropertiesKey(Key = "app-version")]
        private string appVersion;
        private string author;

        public string AppName => appName;

        public string AppVersion => appVersion;

        public string AppAuthor => author;
    }
}