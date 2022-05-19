using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;

using WebAuth;
using WebHoster;

namespace WebApiTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var authInjection = new WebAuthBuilder().AddAllowedPath("/api")
                                                    .AddAllowedPath("/papi")
                                                    .AddAllowedPath("/hubs")
                                                    .AddPolicyClaimMatches("Admin", "Admin", new [] { "true" })
                                                    .AddPolicyClaimMatches("FirstQuarter", "BirthMonth", new [] { "Jan", "Feb", "Mar" })
                                                    .Get();

            var host = new WebBuilder().UseRelaxedCorsPolicy(true)
                                       .UseWebRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebRoot"))
                                       .UseWebAuthInjection(authInjection)
                                       .UseAppInjection(new TestApp.StartupInjection())
                                       .UseAppInjection(new TestSignalRHub.StartupInjection())
                                       .UseFileServer(true)
                                       .UseDefaultFile("index.html")
                                       .Get();
                                       
            
            await host.Build().RunAsync();
        }
    }
}
