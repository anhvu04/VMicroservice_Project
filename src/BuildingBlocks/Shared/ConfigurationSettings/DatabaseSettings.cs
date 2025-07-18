namespace Shared.ConfigurationSettings;

public class DatabaseSettings
{
    public string DbProvider { get; set; } = null!;
    public string DefaultConnection { get; set; } = null!;
}