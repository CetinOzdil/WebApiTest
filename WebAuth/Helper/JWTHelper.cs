using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Http;

using WebHoster.Interface.Authentication;

namespace WebAuth.Helper
{
    public class JwtHelper

    {
        public static string GenerateJwtToken(IUser user, TimeSpan validity)
        {
            // TODO: dont forget to use Config file for this :/
            var key = Encoding.ASCII.GetBytes(Constants.secureKey);

            // generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", user.Id.ToString()), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")) 
                }),
                Expires = DateTime.UtcNow.Add(validity),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static void GenerateCookie(IResponseCookies cookies, string cookieName, string token, TimeSpan validity)
        {
            var cookieOption = new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = new DateTimeOffset(DateTime.UtcNow.Add(TimeSpan.FromDays(7))),
                MaxAge = TimeSpan.FromDays(7),
            };

            cookies.Append(cookieName, token, cookieOption);
        }

        public static int GetUserIdFromToken(string token)
        {
            int userId = -1;

            try
            {
                // TODO: dont forget to use Config file for this :/
                var key = Encoding.ASCII.GetBytes(Constants.secureKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                int.TryParse(jwtToken.Claims.First(x => x.Type == "id").Value, out userId);
            }
            catch 
            {
                // TODO: some exception logging maybe?
            }

            return userId;
        }
    }
}
