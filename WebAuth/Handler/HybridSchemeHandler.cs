using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAuth.Handler
{
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
