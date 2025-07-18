namespace Shared.ConfigurationSettings;

public class GrpcHostSettings
{
    public string InventoryUrl { get; set; } = null!;
    public string ProductUrl { get; set; } = null!;
    public string ScheduledJobUrl { get; set; } = null!;
}