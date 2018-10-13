namespace GitManager.Connection
{
    public class Config : IConfig
    {
        public string BasePath => ConfigurationLoader.Instance.Config.BasePath;
        public string PrivateToken => ConfigurationLoader.Instance.Config.PrivateToken;
        public string FromMail => ConfigurationLoader.Instance.Config.FromMail;
        public string ToMail => ConfigurationLoader.Instance.Config.ToMail;
        public string Host => ConfigurationLoader.Instance.Config.Host;
        public string Username => ConfigurationLoader.Instance.Config.Username;
        public string Password => ConfigurationLoader.Instance.Config.Password;
        public int Port => ConfigurationLoader.Instance.Config.Port;
    }
}