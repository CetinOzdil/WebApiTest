using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebHoster.Interface;

namespace WebHoster
{
    internal class Startup
    {

        private readonly IConfiguration config;
        private readonly IStartupInjectionConfiguration injectionConfig;

        public Startup(IWebHostEnvironment env, IConfiguration configuration, IStartupInjectionConfiguration siConfig)
        {
            HostingEnvironment = env;

            config = configuration;
            injectionConfig = siConfig;
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // if using ssl use redirect too
            if(config.GetValue<bool>("UseSSL"))
                app.UseHttpsRedirection();

            // for testing allow all cors requests
            if (config.GetValue<bool>("RelaxedCors"))
            {
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            app.UseRouting();

            if (injectionConfig.AuthInjection != null)
                injectionConfig.AuthInjection.InjectConfig(app);

            foreach (var item in injectionConfig.ApplicationInjection)
                item.InjectConfig(app);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // if using ssl use redirect too
            if (config.GetValue<bool>("RelaxedCors"))
                services.AddCors();

            services.AddResponseCaching(options =>
            {
                options.SizeLimit = 250;
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            services.AddControllers().AddNewtonsoftJson((options) =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.Formatting = Formatting.None;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            if (injectionConfig.AuthInjection != null)
                injectionConfig.AuthInjection.InjectConfigureServices(services);

            foreach (var item in injectionConfig.ApplicationInjection)
                item.InjectConfigureServices(services);
        }
    }
}
