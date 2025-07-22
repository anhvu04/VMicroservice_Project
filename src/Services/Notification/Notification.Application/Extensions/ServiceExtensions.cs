using Infrastructure.ConfigurationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Notification.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureCqrsMediatR(AssemblyReference.Assembly);
        services.ConfigureMapper();
    }
}