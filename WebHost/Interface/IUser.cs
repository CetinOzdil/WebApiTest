namespace WebHoster.Interface
{
    public interface IUser
    {
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        string Username { get; set; }
    }
}