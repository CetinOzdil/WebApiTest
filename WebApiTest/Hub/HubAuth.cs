using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TestSignalRHub;

namespace TestApp.Hub
{
    public class HubAuth : HubBase
    {
        [Authorize("Admin")]
        public override Task Write(long field, string value)
        {
            return base.Write(field, value);
        }

        [Authorize]
        public override async Task<string> Read(long field)
        {
            return await base.Read(field);
        }
    }
}
