using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.ConfigurationSettings;

namespace Infrastructure.ConfigurationService;

public static class ConfigureGrpcServer
{
    public static void ConfigureGrpcServers(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();

        builder.WebHost.ConfigureKestrel(options =>
        {
            var port = builder.Configuration.GetSection(nameof(GrpcServerPortSettings)).Get<GrpcServerPortSettings>() ??
                       throw new Exception("GrpcServerPortSettings is not configured properly");
            options.ListenAnyIP(port.ServerPortHttp1, o => { o.Protocols = HttpProtocols.Http1; });
            options.ListenAnyIP(port.ServerPortHttp2, o => { o.Protocols = HttpProtocols.Http2; });
        });
    }
}