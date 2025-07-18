using System.Reflection;
using Basket.Application.Common;
using FluentValidation;
using Infrastructure.Behaviors;
using Infrastructure.ConfigurationService;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMapper();
        services.ConfigureCqrsMediatR(AssemblyReference.Assembly);
        services.ConfigureDependencyInjection();
    }

    private static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
    }

    private static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<CartUtils>();
    }
}