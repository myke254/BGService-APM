using BGService_APM.DataAccess;
using BGService_APM.DataAccess.cache;
using BGService_APM.DataAccess.models;
using Microsoft.Extensions.Logging;

namespace BGService_APM.Business
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApiService _weatherApiService;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<WeatherService> _logger;
        public WeatherService(WeatherApiService weatherApiService, ICacheManager cacheManager, ILogger<WeatherService> logger)
        {
            _weatherApiService = weatherApiService;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        public async Task<OpenWeatherMapResponse> GetWeatherData(string cityName)
        {

            // Attempt to retrieve weather data from the cache
            _logger.LogInformation("Fetching weather data from cache");
            OpenWeatherMapResponse? openWeatherMapResponse = await _cacheManager.GetAsync<OpenWeatherMapResponse>(cityName);
            if (openWeatherMapResponse == null)
            {
                _logger.LogInformation("item not found in cache, fetching from openweathermap");
                openWeatherMapResponse = await _weatherApiService.GetWeatherDataAsync(cityName);
            }
            if (openWeatherMapResponse == null) {
                _logger.LogError("Weather data could not be found ");
                throw new Exception("weather data could not be retrieved at the moment"); }
            await _cacheManager.SetAsync(cityName.ToLower(), openWeatherMapResponse);
            return openWeatherMapResponse;
        }
    }

}
