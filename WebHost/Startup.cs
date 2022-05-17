using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebHoster.Interface;

namespace WebHoster
{
    public class Startup
    {
        internal static IStartupInjection AuthInjection { get; set; }
        internal static List<IStartupInjection> ApplicationInjection { get; set; } = new List<IStartupInjection>();
        internal static bool RelaxedCorsPolicy { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            HostingEnvironment = env;
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // for testing allow all cors requests
            if(RelaxedCorsPolicy)
            {
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            app.UseRouting();

            if (AuthInjection != null)
                AuthInjection.InjectConfig(app);

            foreach (var item in ApplicationInjection)
                item.InjectConfig(app);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            if(RelaxedCorsPolicy)
                services.AddCors();

            services.AddControllers().AddNewtonsoftJson((options) =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.Formatting = Formatting.None;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            if (AuthInjection != null)
                AuthInjection.InjectConfigureServices(services);

            foreach (var item in ApplicationInjection)
                item.InjectConfigureServices(services);
        }
    }
}
