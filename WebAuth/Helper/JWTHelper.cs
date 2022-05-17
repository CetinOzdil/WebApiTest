using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebHoster.Interface;

namespace WebAuth.Helper
{
    public class JWTHelper

    {
        public static string GenerateJwtToken(IUser user, TimeSpan validity)
        {
            // generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Constants.secureKey); // dont forget to use Config file for this :)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")) }),
                Expires = DateTime.UtcNow.Add(validity),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
