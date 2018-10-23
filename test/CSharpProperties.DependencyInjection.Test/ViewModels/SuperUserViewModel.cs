using System.IO;
using CSharpProperties.DependencyInjection.Annotations;

namespace CSharpProperties.DependencyInjection.Test.ViewModels
{
    [PropertiesFile(Path = @".\properties-files\superuser.properties")]
    public class SuperUserViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}