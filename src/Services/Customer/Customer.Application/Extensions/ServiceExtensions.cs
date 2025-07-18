using System.Reflection;
using Infrastructure.ConfigurationService;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMapper();
        services.ConfigureCqrsMediatR(AssemblyReference.Assembly);
    }

    private static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
    }
}