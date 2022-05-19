using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace TestSignalRHub
{

    public class TestHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connection : " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Disonnection : " + Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        [Authorize]
        [HubMethodName("GetMessages")]
        public async Task<string> GetMessages()
        {
            return await Task.Run(() => $"I am a string from TestHub, sent at {DateTime.Now:HH:mm:ss}");
        }
    }
}
