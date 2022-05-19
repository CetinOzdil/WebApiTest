using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;

using WebAuth.Middleware;
using WebHoster.Interface;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebAuth.Handler;
using WebAuth.Enum;

namespace WebAuth
{
    internal class StartupInjection : IStartupInjection
    {
        private readonly string loginPath;
        private readonly string logoutPath;
        private readonly Dictionary<string, AllowType> allowedPaths;
        private readonly Dictionary<KeyValuePair<string, string>, string[]> policyClaimPairs;

        public StartupInjection(string login, string logout, Dictionary<string, AllowType> allowed, Dictionary<KeyValuePair<string, string>, string[]> policyPairs)
        {
            loginPath = login;
            logoutPath = logout;
            allowedPaths = allowed;
            policyClaimPairs = policyPairs;
        }

        public void InjectConfig(IApplicationBuilder app)
        {
            app.UseMiddleware<JwtAuthMiddleware>(loginPath, logoutPath, allowedPaths);

            app.UseAuthentication();
            app.UseAuthorization();
        }

        public void InjectConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = "HybridScheme";
                options.DefaultForbidScheme = "HybridScheme";
                options.AddScheme<HybridSchemeHandler>("HybridScheme", "Hybrid Token + Cookie Scheme");
            });

            services.AddAuthorization(options =>
            {
                foreach (var pair in policyClaimPairs)
                    options.AddPolicy(pair.Key.Key, policy => policy.RequireClaim(pair.Key.Value, pair.Value));
            });
        }
    }
}
