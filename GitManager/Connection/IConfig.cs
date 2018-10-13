namespace GitManager.Connection
{
    public interface IConfig
    {
        string BasePath { get; }
        string PrivateToken { get; }
        string FromMail { get; }
        string ToMail { get; }
        string Host { get; }
        string Username { get; }
        string Password { get; }
        int Port { get; }
    }
}