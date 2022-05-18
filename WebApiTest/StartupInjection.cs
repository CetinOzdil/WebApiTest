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
            // define default page as index.html
            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);

            // static file serving
            app.UseFileServer();
            
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddControllers()
                    .AddApplicationPart(Assembly.GetExecutingAssembly())
                    .AddApplicationPart(typeof(TestController.AnotherController).Assembly);
            
        }
    }
}
