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
            // always allow reching to login page
            if (context.Request.Path.Equals(LoginPath, StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(context);
                return;
            }

            // if logout requested, remove cookie and redirect
            // actually this should invalidate token too but
            // for sake of simplicty it is ignored
            if (context.Request.Path.Equals(LogoutPath, StringComparison.InvariantCultureIgnoreCase))
            {
                if (authService.UseCookie)
                    context.Response.Cookies.Delete(authService.CookieName);

                context.Response.Redirect(LoginPath);
                return;
            }

            // check for header for token
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // if token not found and cookies are active check for cookie
            if (string.IsNullOrEmpty(token) && authService.UseCookie)
                context.Request.Cookies.TryGetValue(authService.CookieName, out token);


            // if token is not found
            if (string.IsNullOrEmpty(token))
            {
                // and not making an let contoller to check authentication because it may be calling Login endpoint
                if (context.Request.Path.StartsWithSegments(ApiPrefix, StringComparison.InvariantCultureIgnoreCase))
                    await _next(context);
                else
                    // otherwise redirect to login
                    context.Response.Redirect(LoginPath); 

                return;
            }

            try
            {
                // check token for user id 
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("BEN_GUVENLI_JWT_SECRET_KEYIM"); // sorry for bleeding eyes :)
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

                // check if id is valid
                var user = authService.GetById(userId);

                // if not
                if (user == null)
                {
                    // remove cookie
                    if (authService.UseCookie)
                        context.Response.Cookies.Delete(authService.CookieName);

                    // if this is an api call let controller handle because it may be calling Login endpoint
                    if (context.Request.Path.StartsWithSegments(ApiPrefix, StringComparison.InvariantCultureIgnoreCase))
                        await _next(context);
                    else
                        // otherwise redirect to login
                        context.Response.Redirect(LoginPath);

                    return;
                }

                // add user info to items so controllers & auth attribute could use it
                context.Items["User"] = user;
            }
            catch
            {
                // on exception assume user is not logged in and remove cookie
                if (authService.UseCookie)
                    context.Response.Cookies.Delete(authService.CookieName);
                
                // if this is an api call let controller handle because it may be calling Login endpoint
                if (context.Request.Path.StartsWithSegments(ApiPrefix, StringComparison.InvariantCultureIgnoreCase))
                    await _next(context);
                else
                    // otherwise redirect to login
                    context.Response.Redirect(LoginPath);

                return;
            }

            // you are free to go little request :)
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
