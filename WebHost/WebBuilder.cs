﻿using Microsoft.AspNetCore;
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
        private string webRoot = string.Empty;

        private IPAddress listenIP = IPAddress.Any;
        private int httpPort = 5000;

        private bool useSSL = false;
        private int sslPort = 5001;
        private string sslCertificatePath = string.Empty;
        private string sslCertificatePass = string.Empty;

        private bool relaxedCorsPolicy = false;
        private bool useCompression = false;
        private bool useFileServer = true;
        private string defaultFile = "index.html";


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
            this.webRoot = webRoot;
            return this;
        }

        public WebBuilder UseHttpPort(int port)
        {
            httpPort = port;
            return this;
        }

        public WebBuilder UseListenAddress(IPAddress address)
        {
            listenIP = address;
            return this;
        }

        public WebBuilder UseCompression(bool use)
        {
            useCompression = use;
            return this;
        }

        public WebBuilder UseFileServer(bool use)
        {
            useFileServer = use;
            return this;
        }

        public WebBuilder UseDefaultFile(string file)
        {
            defaultFile = file;
            return this;
        }

        public WebBuilder UseSSL(bool useSSL, int port = 443, string certificatePath = "", string certificatePassword = "")
        {
            this.useSSL = useSSL;
            sslPort = port;
            sslCertificatePath = certificatePath;
            sslCertificatePass = certificatePassword;
            return this;
        }

        public IWebHostBuilder Get()
        {
            // check if ssl is really usable
            var sslUse = useSSL && !string.IsNullOrEmpty(sslCertificatePath) && System.IO.File.Exists(sslCertificatePath);

            var hostBuilder = WebHost.CreateDefaultBuilder();

            // set web root
            if (!string.IsNullOrEmpty(webRoot))
                hostBuilder.UseWebRoot(webRoot);

            // user kestrel for multiplatform support & ssl
            hostBuilder.UseKestrel(options =>
            {
                options.AddServerHeader = false;
                options.Listen(listenIP, httpPort);

                // if using ssl configure it
                if (sslUse)
                {
                    options.Listen(listenIP, sslPort, config =>
                    {
                        if (string.IsNullOrEmpty(sslCertificatePass))
                            config.UseHttps(sslCertificatePath);
                        else
                            config.UseHttps(sslCertificatePath, sslCertificatePass);
                    });
                }
            });

            // pass injection info to startup
            hostBuilder.ConfigureServices(sc => sc.AddSingleton<IStartupInjectionConfiguration>(_startupConfiguration));

            // pass ssl info to startup too
            hostBuilder.UseSetting("UseSSL", sslUse.ToString());
            hostBuilder.UseSetting("SSLport", sslPort.ToString());
            hostBuilder.UseSetting("UseRelaxedCors", relaxedCorsPolicy.ToString());
            hostBuilder.UseSetting("UseCompression", useCompression.ToString());
            hostBuilder.UseSetting("UseFileServer", useFileServer.ToString());
            hostBuilder.UseSetting("DefaultFile", defaultFile.ToString());

            return hostBuilder.UseStartup<Startup>();
        }

    }
}
