using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Notification.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
    }
}