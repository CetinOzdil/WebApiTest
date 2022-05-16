using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTest2
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestContoller : ControllerBase
    {
        [HttpGet]
        [Route("GetTestData")]
        public async Task<string> TestData(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var start = DateTime.Now;

                try
                {
                    Task.Delay(2500).Wait(cancellationToken);
                    Console.WriteLine("Task tamamlandı!");
                    return $"Task started at {start:HH:mm:ss} but now its {DateTime.Now:HH:mm:ss}";
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task cancelled!");
                    return string.Empty;
                }

            }, cancellationToken);
        }

        [HttpGet]
        [Route("GetTestJSONData")]
        public async Task<object> JsonTestData(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var start = DateTime.UtcNow;

                try
                {
                    Task.Delay(500).Wait(cancellationToken);
                    Console.WriteLine("Task tamamlandı!");
                    return new { TaskStart = start, TaskEnd = DateTime.UtcNow, Message = "Task completed!" };
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task cancelled!");
                    return null;
                }

            }, cancellationToken);
        }
    }
}
