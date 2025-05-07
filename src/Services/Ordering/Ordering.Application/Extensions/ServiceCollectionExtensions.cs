using System.Reflection;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.UnitOfWork;

namespace Ordering.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(opt => { opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
        services.AddDependencyInjection();
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
    }
}