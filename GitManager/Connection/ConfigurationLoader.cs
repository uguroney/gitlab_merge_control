using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NLog;

namespace GitManager.Connection
{
    public class ConfigurationLoader
    {
        private static readonly Lazy<ConfigurationLoader> Container =
            new Lazy<ConfigurationLoader>(() => new ConfigurationLoader());

        private readonly DirectoryInfo _executingDirectoryInfo =
            new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "")).Directory;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private ConfigurationLoader()
        {
            LoadConfiguration();
        }

        public static ConfigurationLoader Instance => Container.Value;

        public Configuration Config { get; protected set; }

        private void LoadConfiguration()
        {
            Config = LoadJson<Configuration>("manager.config");
        }

        public void ReLoadConfiguration()
        {
            LoadConfiguration();
        }

        private T LoadJson<T>(string configFile)
        {
            StreamReader streamReader = null;
            try
            {
                if (_executingDirectoryInfo == null)
                {
                    _logger.Error("Cannot get executing directory information.");
                    return default(T);
                }

                streamReader =
                    new StreamReader($"{_executingDirectoryInfo.FullName}{Path.DirectorySeparatorChar}{configFile}");
                var json = streamReader.ReadToEnd();
                var item = JsonConvert.DeserializeObject<T>(json);
                _logger.Debug("Configuration loaded.");
                return item;
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error(ex, "Configuration loading error : Directory not found.");
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Configuration loading error : {ex.Message}.");
                return default(T);
            }
            finally
            {
                streamReader?.Dispose();
            }
        }
    }
}