using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using WebHoster.Interface;

namespace TestSignalRHub
{
    public class StartupInjection<THub> : IStartupInjection where THub : HubBase
    {
        private readonly string endpoint;
        private readonly bool compress;

        public StartupInjection(string hubPath, bool useCompression)
        {
            endpoint = hubPath;
            compress = useCompression;
        }

        public void InjectConfig(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => endpoints.MapHub<THub>(endpoint));
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            var sr = services.AddSignalR();
            
            if(compress)
                sr.AddMessagePackProtocol();

            services.AddHostedService<HubBackgroundService<THub>>();
        }
    }
}
