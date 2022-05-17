using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Reflection;
using WebHoster.Interface;

namespace WebHoster
{
    public class WebBuilder 
    {
        private string _webRoot = string.Empty;

        public WebBuilder UseWebAuthInjection(IStartupInjection startup)
        {
            Startup.AuthInjection = startup;
            return this;
        }

        public WebBuilder UseAppInjection(IStartupInjection startup)
        {
            Startup.ApplicationInjection.Add(startup);
            return this;
        }

        public WebBuilder UseRelaxedCorsPolicy(bool relaxedPolicy)
        {
            Startup.RelaxedCorsPolicy = relaxedPolicy;
            return this;
        }

        public WebBuilder UseWebRoot(string webRoot)
        {
            _webRoot = webRoot;
            return this;
        }

        public IWebHostBuilder Get()
        {
            var hb = WebHost.CreateDefaultBuilder();

            if(!string.IsNullOrEmpty(_webRoot))
                hb.UseWebRoot(_webRoot);

            return hb.UseStartup<Startup>();
        }

    }
}
