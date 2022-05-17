using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiTest2.Entity;
using WebApiTest2.Interface;

namespace WebApiTest2.Service
{
    public class AuthService : IAuthService
    {
        public bool UseCookie { get; set; } = true;
        public string CookieName { get; set; } = "indas_jwt";

        public AuthenticateResponse Authenticate(AuthenticateRequest authRequest)
        {
            if (authRequest == null)
                return null;

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

                return new AuthenticateResponse(user, generateJwtToken(user));
            }

            return null;
        }

        public IEnumerable<User> GetAll()
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

        public User GetById(int id)
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

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("BEN_GUVENLI_JWT_SECRET_KEYIM");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
