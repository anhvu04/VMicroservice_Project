using System.Reflection;
using System.Reflection.Metadata;
using Infrastructure.ConfigurationService;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Mapping;

namespace Product.Application.Extensions;

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
        MappingConfig.RegisterMapping();
    }
}