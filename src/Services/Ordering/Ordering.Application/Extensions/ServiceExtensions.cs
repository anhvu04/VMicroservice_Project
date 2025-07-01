using FluentValidation;
using Infrastructure.Behaviors;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.ConfigureMediatR();
        services.ConfigureMapper();
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

    private static void ConfigureMapper(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
    }
}