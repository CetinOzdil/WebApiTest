using System;
using System.Collections.Generic;
using System.Text;
using WebHoster.Interface;
using WebAuth.Middleware;
using System.Linq;
using WebAuth.Enum;

namespace WebAuth
{
    public class WebAuthBuilder
    {
        private string loginPath = "/login.html";
        private string logoutPath = "/logout";
        private readonly Dictionary<string, PathType> allowedPaths = new Dictionary<string, PathType>();
        private readonly Dictionary<KeyValuePair<string, string>, string[]> policyClaimMathces = new Dictionary<KeyValuePair<string, string>, string[]>();

        public WebAuthBuilder UseLoginPath(string loginPath)
        {
            this.loginPath = loginPath;
            return this;
        }

        public WebAuthBuilder UseLogoutPath(string logoutPath)
        {
            this.logoutPath = logoutPath;
            return this;
        }

        public WebAuthBuilder AddAllowedPath(string path, PathType allowType)
        {
            allowedPaths.TryAdd(path, allowType);
            return this;
        }

        public WebAuthBuilder AddAllowedPaths(IDictionary<string, PathType> paths)
        {
            foreach (var path in paths)
                allowedPaths.TryAdd(path.Key, path.Value);
            
            return this;
        }

        /// <summary>
        /// Add Policy - Claim - Values pair for authorization
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="claim">Claim name</param>
        /// <param name="allowedValues">Allowed claim values for this policy</param>
        /// <returns></returns>
        public WebAuthBuilder AddPolicyClaimMatches(string policy, string claim, IEnumerable<string> allowedValues)
        {
            policyClaimMathces.TryAdd(new KeyValuePair<string, string>(policy, claim), allowedValues.ToArray());

            return this;
        }


        public IStartupInjection Get()
        {
            return new StartupInjection(loginPath, logoutPath, allowedPaths, policyClaimMathces);
        }
    }
}
