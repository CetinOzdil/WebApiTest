namespace WebHoster.Interface
{
    public interface IAuthenticateResponse
    {
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Token { get; set; }
        string Username { get; set; }
    }
}