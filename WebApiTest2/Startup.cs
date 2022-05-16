using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApiTest2
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            HostingEnvironment = env;
        }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);

            app.UseRouting();
            app.UseTestMiddleware();
            app.UseFileServer();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var contentRootPath = HostingEnvironment.ContentRootPath;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }
    }
}
