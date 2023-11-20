using BGService_APM.DataAccess.models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BGService_APM.DataAccess
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<OpenWeatherMapResponse> GetWeatherDataAsync(string cityName)
        {
            string apiKey = _configuration["OpenWeatherMap:Key"] ?? "";

            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string jsonResponse =  await response.Content.ReadAsStringAsync();
            // Handle the possibility of null or empty response
            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new Exception("Empty or null response from the OpenWeatherMap API.");
            }

            OpenWeatherMapResponse openWeatherMapResponse;
            try
            {
                // Attempt to deserialize the JSON response into a WeatherModel object
                openWeatherMapResponse = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(jsonResponse);
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization errors
                throw new Exception("Error deserializing OpenWeatherMap API response.", ex);
            }
            // Check if deserialization was successful
            if (openWeatherMapResponse == null)
            {
                throw new Exception("Unable to deserialize OpenWeatherMap API response.");
            }
            return openWeatherMapResponse;
        }
    }

}
