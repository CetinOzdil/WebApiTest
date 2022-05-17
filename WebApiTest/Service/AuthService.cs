using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestApp.Entity;
using WebHoster.Interface;

namespace TestApp.Service
{
    public class AuthService : IAuthService
    {
        public bool UseCookie { get; set; } = true;
        public string CookieName { get; set; } = "indas_jwt";

        public IAuthenticateResponse Authenticate(IAuthenticateRequest authRequest, HttpContext context)
        {
            if (authRequest == null)
            {
                // clear then cookie
                if (UseCookie)
                    context.Response.Cookies.Delete(this.CookieName);

                return null;
            }

            if (authRequest.Username == "cetin" && authRequest.Password == "123456")
            {
                var user = new User()
                {
                    Username = authRequest.Username,
                    Password = authRequest.Password,
                    FirstName = "Çetin",
                    LastName = "Özdil",
                    Id = 666

                };

                var token = WebAuth.Helper.JWTHelper.GenerateJwtToken(user, TimeSpan.FromDays(7));

                // cookies are active add respons to cookie
                var cookieOption = new CookieOptions()
                {
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = new DateTimeOffset(DateTime.UtcNow.Add(TimeSpan.FromDays(7))),
                    MaxAge = TimeSpan.FromDays(7),
                };
                
                if (UseCookie)
                    context.Response.Cookies.Append(this.CookieName, token, cookieOption);

                return new AuthenticateResponse(user, token);
            }

            // if user could not be found clear the cookie
            if (UseCookie)
                context.Response.Cookies.Delete(this.CookieName);
            
            return null;
        }

        public IEnumerable<IUser> GetAll()
        {
            var result = new List<User>();

            result.Add(new User()
            {
                Username = "cetin",
                Password = "123456",
                FirstName = "Çetin",
                LastName = "Özdil",
                Id = 666

            });

            return result;
        }

        public IUser GetById(int id)
        {
            if(id == 666)
                return new User()
                {
                    Username = "cetin",
                    Password = "123456",
                    FirstName = "Çetin",
                    LastName = "Özdil",
                    Id = 666

                };

            return null;
        }
    }
}
