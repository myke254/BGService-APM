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

                OpenWeatherMapResponse openWeatherMapResponse = await _weatherService.GetWeatherData(cityName);
                return Ok(openWeatherMapResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An exception occurred: {ex.Message}");
            }
        }
    }
}