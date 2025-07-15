namespace Shared.ConfigurationSettings;

public class RedisSettings
{
    public string EndPoints { get; set; } = null!;
    public int Port { get; set; }
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}