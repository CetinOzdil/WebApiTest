using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace WebHoster.Interface.Authentication
{
    public interface IAuthService
    {
        public bool UseCookie { get; set; }
        public string CookieName { get; set; }
        public TimeSpan TokenValidity { get; set; }

        Task<IUser> CheckUser(string username, string pass);
        string GenerateToken(IUser user, TimeSpan validity);
        IAuthenticateResponse GetAuthenticateResponse(IUser user, string token);
        Task<IUser> GetUser(int id);

        public async Task<IAuthenticateResponse> Authenticate(IAuthenticateRequest authRequest, HttpContext context)
        {
            if (authRequest == null)
            {
                context.Response.Cookies.Delete(CookieName);
                return null;
            }

            var user = await CheckUser(authRequest.Username, authRequest.Password);

            if (user == null)
            {
                context.Response.Cookies.Delete(CookieName);
                return null;
            }

            var token = GenerateToken(user, TokenValidity);

            if(UseCookie)
            {
                var cookieOption = new CookieOptions()
                {
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = new DateTimeOffset(DateTime.UtcNow.Add(TokenValidity)),
                    MaxAge = TokenValidity,
                };

                context.Response.Cookies.Append(CookieName, token, cookieOption);
            }

            return GetAuthenticateResponse(user, token);
        }

    }
}
