using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAuth.Handler
{
    /// <summary>
    /// Because of custom authorization ASP.NET Core is always returning 500 for rejection
    /// This class prevents usage of 500 for all and returns 401 or 403 according to situation
    /// </summary>
    public class HybridSchemeHandler : IAuthenticationHandler
    {
        private HttpContext context;

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this.context = context;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
            => Task.FromResult(AuthenticateResult.NoResult());

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            properties ??= new AuthenticationProperties();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            properties ??= new AuthenticationProperties();
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            return Task.CompletedTask;
        }
    }
}
