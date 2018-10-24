# CSharpProperties.DependencyInjection
Dependency Injection for CSharpProperties.

# What is CSharpProperties.DependencyInjection?
`CSharpProperties.DependencyInjection` provides dependency injection to load the properties of a file directly into a class.

# How it works?

### 1. Create a properties file

Create a properties file and save it to the directory of your ASP.NET Core application. see [CSharpProperties](https://github.com/MosaicoSolutions/CSharpProperties).

```
host=localhost
port=123
database=test
```
### 1. Create a class.

```c#
[PropertiesFile(Path = @".\properties-files\db-properties.txt")]
public class DbProperties
{
    [PropertiesKey(Key = "database")]
    public string DatabaseName { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }
}
```
### 3. Configure Services

Use the `AddPropertiesFiles` method to configure the service.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

    services.AddPropertiesFiles();
}
```
