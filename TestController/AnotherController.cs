using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace TestController
{
    [ApiController]
    [Route("papi/[controller]")]
    public class AnotherController : ControllerBase
    {
        [HttpGet]
        [Route("GetString")]
        public async Task<string> GetString(CancellationToken cancellationToken)
        {
            return await Task.Run(() => "I am just a string...", cancellationToken);
        }
    }
}
