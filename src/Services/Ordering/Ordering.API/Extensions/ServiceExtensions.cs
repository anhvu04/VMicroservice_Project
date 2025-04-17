using Microsoft.EntityFrameworkCore;
using Ordering.Persistence.Extensions;
using Ordering.Persistence.Persistence;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddPersistence(configuration);
    }
}