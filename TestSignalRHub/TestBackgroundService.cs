using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestSignalRHub
{
    internal class TestBackgroundService : BackgroundService
    {
        readonly IHubContext<TestHub> hubContext;

        public TestBackgroundService(IHubContext<TestHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await hubContext.Clients.All.SendAsync("ReceiveMessage", $"Merhaba, şu anda saat : {DateTime.Now:HH:mm:ss}", stoppingToken);
                await Task.Delay(1000);
            }
        }
    }
}
