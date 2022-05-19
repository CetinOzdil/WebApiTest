using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using WebHoster.Interface;

namespace TestSignalRHub
{
    public class StartupInjection : IStartupInjection
    {
        public void InjectConfig(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => endpoints.MapHub<TestHub>("/hubs/TestHub"));
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            services.AddSignalR().AddMessagePackProtocol();
            services.AddHostedService<TestBackgroundService>();
        }
    }
}
