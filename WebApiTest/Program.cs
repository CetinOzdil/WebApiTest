using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTest2
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseWebRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"WebRoot"))
                          .UseStartup<Startup>();
        }
    }
}
