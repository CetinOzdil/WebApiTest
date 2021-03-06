using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;

using WebAuth;
using WebAuth.Enum;
using WebHoster;
using TestSignalRHub;
using TestApp.Hub;

namespace WebApiTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var authInjection = new WebAuthBuilder().AddAllowedPath("/api", PathType.Endpoint)
                                                    .AddAllowedPath("/papi", PathType.Endpoint)
                                                    .AddAllowedPath("/hubs", PathType.Endpoint)
                                                    .AddAllowedPath("/anon", PathType.Path)
                                                    .AddPolicyClaimMatches("Admin", "Admin", new [] { "true" })
                                                    .AddPolicyClaimMatches("FirstQuarter", "BirthMonth", new [] { "Jan", "Feb", "Mar" })
                                                    .Get();

            var hubInjection = new HubBuilder<HubAuth>().UseHubPath("/hubs/TestHub")
                                                        .UseBaseTypeName()
                                                        .Get();

            var appInjection = new TestApp.StartupInjection();

            var host = new WebBuilder().UseRelaxedCorsPolicy()
                                       .UseWebRoot(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebRoot"))
                                       .UseDefaultFile("index.html")
                                       .UseFileServer()
                                       .UseWebAuthInjection(authInjection)
                                       .UseAppInjection(appInjection)
                                       .UseAppInjection(hubInjection)
                                       .Get();
                                       
            
            await host.Build().RunAsync();
        }
    }
}
