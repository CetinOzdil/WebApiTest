using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using WebAuth.Enum;
using WebAuth.Helper;
using WebHoster.Interface.Authentication;

namespace WebAuth.Middleware
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string loginPath;
        private readonly string logoutPath;
        private readonly Dictionary<string, PathType> allowedPaths;

        private static string ErrorPage401 = "<html><head><link rel=\"preconnect\" href=\"https://fonts.googleapis.com\"><link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin><link href=\"https://fonts.googleapis.com/css2?family=Roboto&display=swap\" rel=\"stylesheet\"><title>Unauthorized user!</title><style>body{margin:0px; padding:0px; font-family:roboto,consolas,arial,verdana}</style></head><body><div style=\"width:100%; height:75px; background-color:#0388d2; color:white; padding:25px; font-size:60px\"><span>401 Unauthorized</span></div><div style=\"width:100%; height:75px; background-color:#ecfpfc; color:black; padding:25px\"><p>Your user does not have sufficent rights to reach this page.</p><p>You can return to the page you came from by clicking <a href=\"javascript:history.back()\">here</a> or go to login page by clicking <a href=\"##LP##\">here</a></p></div><div style=\"width:100%; height:25px; background-color:#0388d2; color:white; padding-top:7px; padding-left:25px; font-weight:bold\"><span>no worries</span></div></body></html>";
        private static string ErrorPage500 = "<html><head><link rel=\"preconnect\" href=\"https://fonts.googleapis.com\"><link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin><link href=\"https://fonts.googleapis.com/css2?family=Roboto&display=swap\" rel=\"stylesheet\"><title>Unauthorized user!</title><style>body{margin:0px; padding:0px; font-family:roboto,consolas,arial,verdana}</style></head><body><div style=\"width:100%; height:75px; background-color:#0388d2; color:white; padding:25px; font-size:60px\"><span>500 Internal server error</span></div><div style=\"width:100%; height:75px; background-color:#ecfpfc; color:black; padding:25px\"><p>Server encountered an internal error and was unable to complete your request.</p><p>You can return to the page you came from by clicking <a href=\"javascript:history.back()\">here</a> or reloading page by clicking <a href=\"javascript:location.reload();\">here</a></p><p>##ERR##</p></div><div style=\"width:100%; height:25px; background-color:#0388d2; color:white; padding-top:7px; padding-left:25px; font-weight:bold\"><span>no worries</span></div></body></html>";

        public JwtAuthMiddleware(RequestDelegate next, string login, string logout, Dictionary<string, PathType> allowed)
        {
            loginPath = login;
            logoutPath = logout;
            allowedPaths = allowed;

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            // always allow reching to login page or any allowed file path
            if (context.Request.Path.Equals(loginPath, StringComparison.InvariantCultureIgnoreCase) ||
                allowedPaths.Any(p => p.Value == PathType.Path && context.Request.Path.StartsWithSegments(p.Key, StringComparison.InvariantCultureIgnoreCase)))
            {
                await _next(context);
                return;
            }

            // if logout requested, remove cookie and redirect
            // actually this should invalidate token too but
            // for sake of simplicty it is ignored
            if (context.Request.Path.Equals(logoutPath, StringComparison.InvariantCultureIgnoreCase))
            {
                if (authService.UseCookie)
                    context.Response.Cookies.Delete(authService.CookieName);

                context.Response.Redirect(loginPath);
                return;
            }

            string token = string.Empty;

            // check for header for token
            if (context.Request.Headers.TryGetValue("Authorization", out var bearer) && bearer.Count > 0)
                token = bearer[0].Split(" ").LastOrDefault();

            // if token not found and cookies are active check for cookie
            if (string.IsNullOrEmpty(token) && authService.UseCookie)
                context.Request.Cookies.TryGetValue(authService.CookieName, out token);

            // if token is not found
            if (string.IsNullOrEmpty(token))
            {
                // and it's an allowed endpoint then let contoller to check authentication
                if (allowedPaths.Any(p => p.Value == PathType.Endpoint && context.Request.Path.StartsWithSegments(p.Key, StringComparison.InvariantCultureIgnoreCase)))
                    await _next(context);
                else
                {
                    context.Response.ContentType = "text/html";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(ErrorPage401.Replace("##LP##", loginPath));
                }

                return;
            }

            try
            {
                // check token for user id 
                var userId = JwtHelper.GetUserIdFromToken(token);

                // check if id is valid
                var user = await authService.GetUser(userId);

                // if not
                if (user == null)
                {
                    // remove cookie
                    if (authService.UseCookie)
                        context.Response.Cookies.Delete(authService.CookieName);

                    // and if request path is an allowed endpoint then let contoller to check authentication
                    if (allowedPaths.Any(p => p.Value == PathType.Endpoint && context.Request.Path.StartsWithSegments(p.Key, StringComparison.InvariantCultureIgnoreCase)))
                        await _next(context);
                    else
                    {
                        context.Response.ContentType = "text/html";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(ErrorPage401.Replace("##LP##", loginPath));
                    }

                    return;
                }

                // set user identity and isauthorised to true so
                // controllers & hubs could use it for auth
                var identity = new ClaimsIdentity(user.Claims, "Custom");
                context.User = new ClaimsPrincipal(identity);
            }
            catch(Exception ex)
            {
                // on exception assume user is not logged in and remove cookie
                if (authService.UseCookie)
                    context.Response.Cookies.Delete(authService.CookieName);

                // and if request path is an allowed endpoint then let contoller to check authentication
                if (allowedPaths.Any(p => p.Value == PathType.Endpoint && context.Request.Path.StartsWithSegments(p.Key, StringComparison.InvariantCultureIgnoreCase)))
                    await _next(context);
                else
                {
                    // send internal server error
                    context.Response.ContentType = "text/html";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(ErrorPage500.Replace("##ERR##", ex.Message));
                }

                return;
            }

            // you are free to go little request :)
            await _next(context);
        }
    }
}
