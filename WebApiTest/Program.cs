using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebHoster;
using WebAuth;

namespace WebApiTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var authInjection = new WebAuthBuilder().AddAllowedPath("/api")
                                                    .Get();

            //                                                     .AddAllowedPath("/papi")                                    


            var host = new WebBuilder().UseRelaxedCorsPolicy(true)
                                       .UseWebRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebRoot"))
                                       .UseWebAuthInjection(authInjection)
                                       .UseAppInjection(new TestApp.StartupInjection())
                                       .UseSSL(true)
                                       .Get();
                                       
            
            await host.Build().RunAsync();
        }
    }
}
