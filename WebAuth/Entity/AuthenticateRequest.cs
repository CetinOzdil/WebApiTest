using System.ComponentModel.DataAnnotations;

using WebHoster.Interface.Authentication;

namespace WebAuth.Entity
{
    public class AuthenticateRequest : IAuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
