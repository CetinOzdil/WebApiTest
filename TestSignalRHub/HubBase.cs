using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace TestSignalRHub
{
    public class HubBase : Hub
    {
        public static bool UseBaseTypeName { get; internal set; } = false;
        public static bool CheckAfterWrite { get; internal set; } = false;

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

        [HubMethodName("init")]
        public async Task<string> Init()
        {
            return await Task.Run(() => "Inited!");
        }

        [HubMethodName("write")]
        public virtual async Task Write(long field, string value)
        {
            await Task.Run(() => Console.WriteLine($"Write -> Field : {field}, Value : {value}"));
        }

        [HubMethodName("read")]
        public virtual async Task<string> Read(long field)
        {
            return await Task.Run(() => $"I am value of {field}");
        }
    }
}
