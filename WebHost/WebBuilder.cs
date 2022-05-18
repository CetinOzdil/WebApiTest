using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using WebHoster.Interface;
using WebHoster.Class;
using Microsoft.Extensions.DependencyInjection;

namespace WebHoster
{
    public class WebBuilder
    {
        private string _webRoot = string.Empty;
        private bool _useSSL = false;
        private int _sslPort = 5001;
        private int _httpPort = 5000;
        private string _certificatePath = string.Empty;
        private string _certificatePass = string.Empty;
        private IPAddress _listenIP = IPAddress.Any;

        private bool relaxedCorsPolicy = false;

        private readonly StartupInjectionConfiguration _startupConfiguration = new StartupInjectionConfiguration();

        public WebBuilder UseWebAuthInjection(IStartupInjection startup)
        {
            _startupConfiguration.AuthInjection = startup;
            return this;
        }

        public WebBuilder UseAppInjection(IStartupInjection startup)
        {
            _startupConfiguration.ApplicationInjection.Add(startup);
            return this;
        }

        public WebBuilder UseRelaxedCorsPolicy(bool relaxedPolicy)
        {
            relaxedCorsPolicy = relaxedPolicy;
            return this;
        }

        public WebBuilder UseWebRoot(string webRoot)
        {
            _webRoot = webRoot;
            return this;
        }

        public WebBuilder UseHttpPort(int port)
        {
            _httpPort = port;
            return this;
        }

        public WebBuilder UseListenAddress(IPAddress address)
        {
            _listenIP = address;
            return this;
        }

        public WebBuilder UseSSL(bool useSSL, int port = 443, string certificatePath = "", string certificatePassword = "")
        {
            _useSSL = useSSL;
            _sslPort = port;
            _certificatePath = certificatePath;
            _certificatePass = certificatePassword;
            return this;
        }

        public IWebHostBuilder Get()
        {
            // check if ssl is really usable
            var sslUse = _useSSL && !string.IsNullOrEmpty(_certificatePath) && System.IO.File.Exists(_certificatePath);

            var hb = WebHost.CreateDefaultBuilder();

            // set web root
            if (!string.IsNullOrEmpty(_webRoot))
                hb.UseWebRoot(_webRoot);

            // user kestrel for multiplatform support & ssl
            hb.UseKestrel(options =>
            {
                options.AddServerHeader = false;
                options.Listen(_listenIP, _httpPort);

                // if using ssl configure it
                if (sslUse)
                {
                    options.Listen(_listenIP, _sslPort, config =>
                    {
                        if (string.IsNullOrEmpty(_certificatePass))
                            config.UseHttps(_certificatePath);
                        else
                            config.UseHttps(_certificatePath, _certificatePass);
                    });
                }
            });

            // pass injection info to startup
            hb.ConfigureServices(sc => sc.AddSingleton<IStartupInjectionConfiguration>(_startupConfiguration));

            // pass ssl info to startup too
            hb.UseSetting("UseSSL", sslUse.ToString());
            hb.UseSetting("RelaxedCors", relaxedCorsPolicy.ToString());

            return hb.UseStartup<Startup>();
        }

    }
}
