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
    public class StartupInjection2 : IStartupInjection
    {
        public void InjectConfig(IApplicationBuilder app)
        {
            //
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().AddApplicationPart(typeof(TestController.FarkliController).Assembly).AddControllersAsServices();
        }
    }
}
