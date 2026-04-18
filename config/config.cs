namespace config;

static class Config
{
    public static ConfigurationManager AddAppSettings(this ConfigurationManager configuration)
    {
        configuration.AddJsonFile("config/appsettings.json", optional: false, reloadOnChange: true);
        return configuration;
    }
}
