using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TestApp.Service;
using WebHoster.Interface;
using WebHoster.Interface.Authentication;

namespace TestApp
{
    public class StartupInjection : IStartupInjection
    {
        public void InjectConfig(IApplicationBuilder app)
        {
            //
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthService>();

            services.AddControllers()
                    .AddApplicationPart(Assembly.GetExecutingAssembly())
                    .AddApplicationPart(typeof(TestController.AnotherController).Assembly);

        }
    }
}
