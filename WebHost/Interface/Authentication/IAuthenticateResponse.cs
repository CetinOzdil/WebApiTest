namespace WebHoster.Interface.Authentication
{
    public interface IAuthenticateResponse
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Token { get; set; }
    }
}