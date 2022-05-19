using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public Startup(IWebHostEnvironment env, IConfiguration configuration, IStartupInjectionConfiguration startupInjectionConfig)
        {
            HostingEnvironment = env;

            config = configuration;
            injectionConfig = startupInjectionConfig;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // relaxed cors
            if (config.GetValue<bool>("UseRelaxedCors"))
                services.AddCors();

            // ssl redirect
            if (config.GetValue<bool>("UseSSL"))
            {
                var sslPort = config.GetValue<int>("SSLport");

                services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = sslPort;
                    options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
                });
            }

            services.AddResponseCaching(options =>
            {
                options.SizeLimit = 250;
            });

            // use compression
            if (config.GetValue<bool>("UseCompression"))
            {
                services.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.Providers.Add<BrotliCompressionProvider>();
                });
            }

            // json config
            services.AddControllers().AddNewtonsoftJson((options) =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.Formatting = Formatting.None;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // auth injections
            if (injectionConfig.AuthInjection != null)
                injectionConfig.AuthInjection.InjectConfigureServices(services);

            // app injections
            foreach (var item in injectionConfig.ApplicationInjection)
                item.InjectConfigureServices(services);
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // if using ssl use redirect too
            if (config.GetValue<bool>("UseSSL"))
                app.UseHttpsRedirection();

            // for testing allow all cors requests
            if (config.GetValue<bool>("UseRelaxedCors"))
            {
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            app.UseRouting();

            // auth injections
            if (injectionConfig.AuthInjection != null)
                injectionConfig.AuthInjection.InjectConfig(app);

            // app injections
            foreach (var item in injectionConfig.ApplicationInjection)
                item.InjectConfig(app);


            // define default page as index.html
            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add(config.GetValue<string>("DefaultFile"));
            app.UseDefaultFiles(options);

            // static file serving
            if (config.GetValue<bool>("UseFileServer"))
                app.UseFileServer();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

    }
}
