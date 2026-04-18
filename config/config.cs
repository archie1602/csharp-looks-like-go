namespace config;

static class Config
{
    public static ConfigurationManager AddAppSettings(this ConfigurationManager configuration)
    {
        configuration.AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
        return configuration;
    }

    public static string GetPostgresConnectionString(this IConfiguration configuration) =>
        configuration.GetConnectionString("DefaultConnection") ??
        configuration["DATABASE_URL"] ??
        "Host=localhost;Port=5432;Database=minimal_web_api;Username=postgres;Password=postgres";
}
