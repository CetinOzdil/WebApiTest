using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiTest2.Interface;

namespace WebApiTest2.Middleware
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public static string LoginPath { get; set; } = "/login.html";
        public static string LogoutPath { get; set; } = "/logout";
        public static string ApiPrefix { get; set; } = "/api";
        

        public JwtAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            if (context.Request.Path.Equals(LoginPath, StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.Equals(LogoutPath, StringComparison.InvariantCultureIgnoreCase))
            {
                if (authService.UseCookie)
                    context.Response.Cookies.Delete(authService.CookieName);

                context.Response.Redirect(LoginPath);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token) || authService.UseCookie)
                context.Request.Cookies.TryGetValue(authService.CookieName, out token);


            if (string.IsNullOrEmpty(token))
            {
                if (context.Request.Path.StartsWithSegments(ApiPrefix, StringComparison.InvariantCultureIgnoreCase))
                    await _next(context);
                else
                    context.Response.Redirect(LoginPath);

                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("BEN_GUVENLI_JWT_SECRET_KEYIM");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                int.TryParse(jwtToken.Claims.First(x => x.Type == "id").Value, out int userId);

                var user = authService.GetById(userId);

                if (user == null)
                {
                    if (authService.UseCookie)
                        context.Response.Cookies.Delete(authService.CookieName);

                    if (context.Request.Path.StartsWithSegments(ApiPrefix, StringComparison.InvariantCultureIgnoreCase))
                        await _next(context);
                    else
                        context.Response.Redirect(LoginPath);

                    return;
                }

                context.Items["User"] = user;
            }
            catch
            {
                if (authService.UseCookie)
                    context.Response.Cookies.Delete(authService.CookieName);

                if (context.Request.Path.StartsWithSegments(ApiPrefix, StringComparison.InvariantCultureIgnoreCase))
                    await _next(context);
                else
                    context.Response.Redirect(LoginPath);

                return;
            }

            await _next(context);
        }
    }

    public static class JwtAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtAuthMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthMiddleware>();
        }
    }
}
