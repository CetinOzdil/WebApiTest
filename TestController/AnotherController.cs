using Microsoft.AspNetCore.Mvc;

namespace TestController
{
    [ApiController]
    [Route("papi/[controller]")]
    public class AnotherController : ControllerBase
    {
        [HttpGet]
        [Route("GetString")]
        public IActionResult GetString()
        {
            return Ok("I am just a string...");
        }
    }
}
