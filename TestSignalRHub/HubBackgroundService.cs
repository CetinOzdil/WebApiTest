﻿using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace TestSignalRHub
{
    internal class HubBackgroundService<THub> : BackgroundService where THub : HubBase
    {
        readonly IHubContext<THub> hubContext;

        public HubBackgroundService(IHubContext<THub> hubContext)
        {
            this.hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", $"Merhaba, şu anda saat : {DateTime.Now:HH:mm:ss} -- UseBase : {HubBase.UseBaseTypeName} - CAW : {HubBase.CheckAfterWrite}", stoppingToken);
                await Task.Delay(1000);
            }
        }
    }
}
