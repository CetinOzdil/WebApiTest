using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public List<User> Users { get; } = new List<User>()
        {
            new User() {
                            Username = "cetin",
                            Password = "123456",
                            FirstName = "Çetin",
                            LastName = "Özdil",
                            Id = 1,
                            Claims = new List<Claim>()
                            {
                                new Claim("Admin", "true"),
                                new Claim("BirthMonth", "Apr")
                            }
                        },
            new User() {
                            Username = "koray",
                            Password = "123456",
                            FirstName = "Koray",
                            LastName = "Kısa",
                            Id = 2,
                            Claims = new List<Claim>()
                            {
                                new Claim("Admin", "true"),
                                new Claim("BirthMonth", "Jan")
                            }
                        },
        };

        public async Task<IUser> CheckUser(string username, string pass)
        {
            return await Task.Run(() =>
            {
                return Users.FirstOrDefault(a => a.Username == username && a.Password == pass);
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
                return Users.FirstOrDefault(a => a.Id == id);
            });
        }
    }
}
