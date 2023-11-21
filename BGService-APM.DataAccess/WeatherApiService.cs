using BGService_APM.DataAccess.models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BGService_APM.DataAccess
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherApiService> _logger;

        public WeatherApiService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherApiService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<OpenWeatherMapResponse> GetWeatherDataAsync(string cityName)
        {
            string apiKey = _configuration["OpenWeatherMap:Key"] ?? "";

            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            //response.EnsureSuccessStatusCode();
            string jsonResponse =  await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonResponse))
            {
                _logger.LogCritical($"Something went wrong. no response received");
                throw new Exception("Empty or null response from the OpenWeatherMap API.");
            }
            if (JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonResponse)!.TryGetValue("cod", out dynamic? codValue))
            {

                if (codValue != 200)
                {
                    _logger.LogError($"Check Details:: {jsonResponse}");
                    throw new Exception($"Something went wrong {jsonResponse}");
                }
            }
            OpenWeatherMapResponse openWeatherMapResponse;
            try
            {
                openWeatherMapResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(jsonResponse)!;
            }
            catch (JsonException ex)
            {
                throw new Exception("Error deserializing OpenWeatherMap API response.", ex);
            }
            if (openWeatherMapResponse == null)
            {
                throw new Exception("Unable to deserialize OpenWeatherMap API response.");
            }
            return openWeatherMapResponse;
        }
    }

}
