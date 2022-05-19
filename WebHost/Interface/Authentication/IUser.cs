using System.Collections.Generic;
using System.Security.Claims;

namespace WebHoster.Interface.Authentication
{
    public interface IUser
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        List<Claim> Claims { get; set; }
    }
}