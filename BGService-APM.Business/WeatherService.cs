using BGService_APM.DataAccess;
using BGService_APM.DataAccess.models;

namespace BGService_APM.Business
{
    public class WeatherService
    {
        private readonly WeatherApiService _weatherApiService;

        public WeatherService(WeatherApiService weatherApiService)
        {
            _weatherApiService = weatherApiService;
        }

        public async Task<OpenWeatherMapResponse> GetWeatherData(string cityName)
        {
            // Add any additional business logic if needed
            return await _weatherApiService.GetWeatherDataAsync(cityName);
        }
    }

}
