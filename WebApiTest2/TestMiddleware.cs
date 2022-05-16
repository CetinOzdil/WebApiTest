using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApiTest2
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.Request.Path == "/login.html")
            {
                await _next(context);
                return;
            }

            if(!context.Request.Cookies.TryGetValue("testCookie", out string cookie) || string.IsNullOrEmpty(cookie))
            {
                context.Response.Redirect("/login.html");
                return;
            }

            await _next(context);
        }
    }

    public static class TestMiddlewareMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>();
        }
    }
}
