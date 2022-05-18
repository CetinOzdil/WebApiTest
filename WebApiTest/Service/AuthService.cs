using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Entity;
using WebHoster.Interface.Authentication;

namespace TestApp.Service
{
    public class AuthService : IAuthService
    {
        public bool UseCookie { get; set; } = true;
        public string CookieName { get; set; } = "indas_jwt";
        public TimeSpan TokenValidity { get; set; } = TimeSpan.FromDays(7);

        public async Task<IUser> CheckUser(string username, string pass)
        {
            return await Task.Run(() =>
            {
                if (username == "cetin" && pass == "123456")
                {
                    return new User()
                    {
                        Username = "cetin",
                        Password = "123456",
                        FirstName = "Çetin",
                        LastName = "Özdil",
                        Id = 666

                    };
                }

                return null;
            });
        }

        public string GenerateToken(IUser user, TimeSpan validity)
        {
            return WebAuth.Helper.JwtHelper.GenerateJwtToken(user, validity);
        }

        public IAuthenticateResponse GetAuthenticateResponse(IUser user, string token)
        {
            return new AuthenticateResponse(user, token);
        }

        public async Task<IUser> GetUser(int id)
        {
            return await Task.Run(() =>
            {
                if (id == 666)
                {
                    return new User()
                    {
                        Username = "cetin",
                        Password = "123456",
                        FirstName = "Çetin",
                        LastName = "Özdil",
                        Id = 666

                    };
                }

                return null;
            });
        }
    }
}
