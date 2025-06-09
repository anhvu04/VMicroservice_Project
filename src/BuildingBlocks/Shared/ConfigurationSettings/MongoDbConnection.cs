namespace Shared.ConfigurationSettings;

public class MongoDbConnection
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}