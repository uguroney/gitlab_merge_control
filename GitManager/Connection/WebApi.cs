using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace GitManager.Connection
{
    public class WebApi
    {
        private static readonly Lazy<WebApi> Container = new Lazy<WebApi>(() => new WebApi());
        private readonly HttpClient _client;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private WebApi()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _client = new HttpClient();
        }

        public static WebApi Instance => Container.Value;

        public async Task<TOut> GetAsync<TOut>(string apiCall, IConfig config)
        {
            _client.DefaultRequestHeaders.Add("Private-Token", config.PrivateToken);
            var path = $"{config.BasePath}{apiCall}";
            try
            {
                var response = await _client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<TOut>(json);
                    return result;
                }

                _logger.Error($"Request failed : {response.StatusCode}");

                return default(TOut);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Get api call failed.");
                return default(TOut);
            }
        }
    }
}