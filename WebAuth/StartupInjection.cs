using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using WebHoster.Interface;
using WebAuth.Middleware;

namespace WebAuth
{
    public class StartupInjection : IStartupInjection
    {
        public void InjectConfig(IApplicationBuilder app)
        {
            app.UseJwtAuthMiddleware();
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            //
        }
    }
}
