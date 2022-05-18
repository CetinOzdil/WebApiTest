using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;

using WebAuth.Middleware;
using WebHoster.Interface;

namespace WebAuth
{
    internal class StartupInjection : IStartupInjection
    {
        public void InjectConfig(IApplicationBuilder app)
        {
            app.UseMiddleware<JwtAuthMiddleware>();
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            //
        }
    }
}
