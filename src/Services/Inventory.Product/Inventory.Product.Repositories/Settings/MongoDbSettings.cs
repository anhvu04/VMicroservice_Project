namespace Inventory.Product.Repositories.Settings;

public class MongoDbSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}