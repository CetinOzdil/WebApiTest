namespace WebHoster.Interface
{
    public interface IAuthenticateRequest
    {
        string Password { get; set; }
        string Username { get; set; }
    }
}