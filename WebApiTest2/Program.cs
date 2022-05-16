using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApiTest2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();

            await Task.Delay(-1);
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseKestrel()
                          .UseWebRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"WebRoot"))
                          .UseStartup<Startup>();
        }
    }
}
