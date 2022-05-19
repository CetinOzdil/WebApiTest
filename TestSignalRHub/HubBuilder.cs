using System;
using System.Collections.Generic;
using System.Text;
using WebHoster.Interface;
using WebAuth.Middleware;
using System.Linq;

namespace TestSignalRHub
{
    /// <summary>
    /// Reuturns needed startup injections for a SignalR Hub 
    /// </summary>
    /// <typeparam name="THub">For standart usage use HubBase class, for authorization inherit HubBase and add needed attributes to class/methods</typeparam>
    public class HubBuilder<THub> where THub : HubBase
    {
        private string hubPath = "/SC";
        private bool useCompression = false;

        public HubBuilder<THub> UseHubPath(string hubPath)
        {
            this.hubPath = hubPath;
            return this;
        }

        public HubBuilder<THub> UseCompression()
        {
            useCompression = true;
            return this;
        }

        public HubBuilder<THub> UseCheckAfterWrite()
        {
            HubBase.CheckAfterWrite = true;
            return this;
        }

        public HubBuilder<THub> UseBaseTypeName()
        {
            HubBase.UseBaseTypeName = true;
            return this;
        }

        public StartupInjection<THub> Get()
        {
            return new StartupInjection<THub>(hubPath, useCompression);
        }
    }
}
