using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ConfigurationService;

public static class ConfigureMapster
{
    public static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
    }
}