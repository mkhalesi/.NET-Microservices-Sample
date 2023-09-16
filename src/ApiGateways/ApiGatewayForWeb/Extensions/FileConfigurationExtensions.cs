using Ocelot.Configuration.File;

namespace ApiGateway.ForWeb.Extensions
{
    public static class FileConfigurationExtensions
    {
        public static IServiceCollection ConfigureDownstreamHostAndPortsPlaceholders(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.PostConfigure<FileConfiguration>(fileConfiguration =>
            {
                var globalHosts = configuration
                    .GetSection($"{nameof(FileConfiguration.GlobalConfiguration)}:Hosts")
                    .Get<GlobalHosts>();

                foreach (var route in fileConfiguration.Routes)
                {
                    ConfigureRote(route, globalHosts);
                }
            });

            return services;
        }

        private static void ConfigureRote(FileRoute route, GlobalHosts globalHosts)
        {
            foreach (var hostAndPort in route.DownstreamHostAndPorts)
            {
                var host = hostAndPort.Host;
                if (host.StartsWith("{") && host.EndsWith("}"))
                {
                    var placeHolder = host.TrimStart('{').TrimEnd('}');
                    if (globalHosts.TryGetValue(placeHolder, out var uri))
                    {
                        route.DownstreamScheme = uri.Scheme;
                        hostAndPort.Host = uri.Host;
                        hostAndPort.Port = uri.Port;
                    }
                }
            }
        }
    }

    public class GlobalHosts : Dictionary<string, Uri> { }

}
