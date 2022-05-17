using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebHoster;

namespace WebApiTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = new WebBuilder().UseRelaxedCorsPolicy(true)
                                       .UseWebRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebRoot"))
                                       .UseWebAuthInjection(new WebAuth.StartupInjection())
                                       .UseAppInjection(new TestApp.StartupInjection())
                                       .Get()
                                       .UseUrls(@"http://localhost:5000");
            
            var host2 = new WebBuilder().UseRelaxedCorsPolicy(true)
                                       .UseAppInjection(new TestApp.StartupInjection2())
                                       .Get()
                                       .UseUrls(@"http://localhost:5001");

            await host.Build().StartAsync();
            await host2.Build().RunAsync();

            //await Task.Delay(Timeout.Infinite);
        }
    }
}
