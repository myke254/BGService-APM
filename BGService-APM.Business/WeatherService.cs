using BGService_APM.DataAccess;
using BGService_APM.DataAccess.cache;
using BGService_APM.DataAccess.models;

namespace BGService_APM.Business
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApiService _weatherApiService;
        private readonly ICacheManager _cacheManager;
        public WeatherService(WeatherApiService weatherApiService, ICacheManager cacheManager)
        {
            _weatherApiService = weatherApiService;
            _cacheManager = cacheManager;
        }

        public async Task<OpenWeatherMapResponse> GetWeatherData(string cityName)
        {
            // Add any additional business logic if needed
            OpenWeatherMapResponse openWeatherMapResponse = await _weatherApiService.GetWeatherDataAsync(cityName);
            if (openWeatherMapResponse == null)
            {
                throw new Exception("weather data could not be retrieved at the moment");
            }
            await _cacheManager.SetAsync(cityName.ToLower(), openWeatherMapResponse);
            return openWeatherMapResponse;
        }
    }

}
