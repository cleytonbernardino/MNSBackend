using Microsoft.Extensions.Configuration;

namespace MMS.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("Connection")!;

    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        string? inMemoryTest = configuration.GetSection("InMemoryTest").Value;
        return !string.IsNullOrEmpty(inMemoryTest);
    }
}
