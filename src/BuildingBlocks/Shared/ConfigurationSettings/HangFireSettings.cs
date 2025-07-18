namespace Shared.ConfigurationSettings;

public class HangFireSettings
{
    public string Route { get; set; } = null!;
    public string ServerName { get; set; } = null!;
    public DashBoard DashBoard { get; set; } = null!;
    public DatabaseSettings DatabaseSettings { get; set; } = null!;
}

public class DashBoard
{
    public string AppPath { get; set; } = null!;
    public int StatsPollingInterval { get; set; }
    public string DashboardTitle { get; set; } = null!;
}