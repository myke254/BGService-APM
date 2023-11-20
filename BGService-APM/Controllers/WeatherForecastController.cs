using BGService_APM.Business;
using BGService_APM.DataAccess.cache;
using BGService_APM.DataAccess.models;
using Microsoft.AspNetCore.Mvc;

namespace BGService_APM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;
        private readonly ICacheManager _cacheManager;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService, ICacheManager cacheManager)
        {
            _logger = logger;
            _weatherService = weatherService;
            _cacheManager = cacheManager;
        }

        [HttpGet("{cityName}")]
        public async Task<ActionResult<OpenWeatherMapResponse>> GetWeatherData(string cityName)
        {
            try
            {
                // Attempt to retrieve weather data from the cache
                OpenWeatherMapResponse? weatherData = await _cacheManager.GetAsync<OpenWeatherMapResponse>(cityName);

                if (weatherData != null)
                {
                    // If data is found in the cache, return it
                    return Ok(weatherData);
                }
                else
                {
                    // If not found in the cache, fetch from the API
                    weatherData = await _weatherService.GetWeatherData(cityName);

                    // The data is already stored in the cache by the WeatherApiService
                    return Ok(weatherData);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}