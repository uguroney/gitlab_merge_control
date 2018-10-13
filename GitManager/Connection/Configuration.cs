namespace GitManager.Connection
{
    public class Configuration
    {
        public string BasePath { get; set; }
        public string PrivateToken { get; set; }
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}