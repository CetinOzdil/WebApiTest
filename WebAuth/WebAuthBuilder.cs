using System;
using System.Collections.Generic;
using System.Text;
using WebHoster.Interface;
using WebAuth.Middleware;

namespace WebAuth
{
    public class WebAuthBuilder
    {
        public WebAuthBuilder()
        {
            JwtAuthMiddleware.AllowedPaths.Add("/api");
        }

        public WebAuthBuilder UseLoginPath(string loginPath)
        {
            JwtAuthMiddleware.LoginPath = loginPath;
            return this;
        }

        public WebAuthBuilder UseLogoutPath(string logoutPath)
        {
            JwtAuthMiddleware.LogoutPath = logoutPath;
            return this;
        }

        public WebAuthBuilder AddAllowedPath(string path)
        {
            if (JwtAuthMiddleware.AllowedPaths.Count == 0 && JwtAuthMiddleware.AllowedPaths[0] == "/api")
                JwtAuthMiddleware.AllowedPaths.Clear();

            JwtAuthMiddleware.AllowedPaths.Add(path);

            return this;
        }

        public WebAuthBuilder AddAllowedPaths(IEnumerable<string> paths)
        {
            if (JwtAuthMiddleware.AllowedPaths.Count == 0 && JwtAuthMiddleware.AllowedPaths[0] == "/api")
                JwtAuthMiddleware.AllowedPaths.Clear();

            JwtAuthMiddleware.AllowedPaths.AddRange(paths);

            return this;
        }


        public IStartupInjection Get()
        {
            return new StartupInjection();
        }
    }
}
