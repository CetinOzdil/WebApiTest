using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Text;
using TestApp.Service;
using WebApiTest.Controller;
using WebHoster.Interface;

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
            services.AddControllers();
            services.AddMvc().AddApplicationPart(Assembly.GetExecutingAssembly()).AddControllersAsServices();
            services.AddMvc().AddApplicationPart(typeof(TestController.FarkliController).Assembly).AddControllersAsServices();
        }
    }
}
