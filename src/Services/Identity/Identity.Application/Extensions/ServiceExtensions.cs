using System.Reflection.Metadata;
using FluentValidation;
using Infrastructure.Behaviors;
using Infrastructure.ConfigurationService;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.ConfigureCqrsMediatR(AssemblyReference.Assembly);
        services.ConfigureMapper();
    }
}