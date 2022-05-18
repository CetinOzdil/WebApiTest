namespace WebHoster.Interface.Authentication
{
    public interface IAuthenticateRequest
    {
        string Password { get; set; }
        string Username { get; set; }
    }
}