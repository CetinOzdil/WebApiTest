﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTest.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("GetTestData")]
        public async Task<string> TestData(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var start = DateTime.Now;

                try
                {
                    Task.Delay(TimeSpan.FromSeconds(3)).Wait(cancellationToken);
                    Console.WriteLine("Task completed!");
                    return $"Task started at {start:HH:mm:ss} but now its {DateTime.Now:HH:mm:ss} and it's completed!";
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task cancelled!");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task cancelled!");
                }

                return string.Empty;
            }, cancellationToken);
        }

        [Authorize("FirstQuarter")]
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
                    Console.WriteLine("Task completed!");
                    return new { TaskStart = start, TaskEnd = DateTime.UtcNow, Message = "Task completed!" };
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task cancelled!");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task cancelled!");
                }

                return null;
            }, cancellationToken);
        }

        [HttpGet]
        [Route("GetTestAnonymousData")]
        public async Task<string> GetAnonData(CancellationToken cancellationToken)
        {
            return await Task.Run(() => "Sorry i don't have so much info for you", cancellationToken);
        }
    }
}
