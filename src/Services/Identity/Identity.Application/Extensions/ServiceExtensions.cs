using System.Reflection.Metadata;
using FluentValidation;
using Infrastructure.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.ConfigureMediatR();
    }

    private static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            opt.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
    }
}