using System.Reflection;
using System.Reflection.Metadata;
using FluentValidation;
using Infrastructure.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ConfigurationService;

public static class ConfigureMediatR
{
    public static void ConfigureCqrsMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(assembly);
            opt.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly);
    }
}