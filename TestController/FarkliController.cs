using Microsoft.AspNetCore.Mvc;
using System;

namespace TestController
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarkliController : ControllerBase
    {
        [HttpGet]
        [Route("GetString")]
        public IActionResult GetString()
        {
            return Ok("ben bir stringim");
        }

    }
}
