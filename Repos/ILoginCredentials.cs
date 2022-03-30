namespace DataBaseTest.Repos
{
    public interface ILoginCredentials
    {
        string PersonalOnlineKey { get; }
        string Username { get; }
        string Password { get; }
    }
}
