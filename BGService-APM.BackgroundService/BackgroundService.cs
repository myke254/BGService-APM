using BGService_APM.Business;
using BGService_APM.DataAccess.models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace BGService_APM.BackgroundService
{

    public class WeatherWorkerService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<WeatherWorkerService> _logger;
        private readonly IWeatherService _weatherService;


        public WeatherWorkerService(
            ILogger<WeatherWorkerService> logger,
            IWeatherService weatherService
            )
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string cityName = "Nairobi";
                    OpenWeatherMapResponse weatherData = await _weatherService.GetWeatherData(cityName);
                    _logger.LogInformation($"Weather Data:: {JsonConvert.SerializeObject(weatherData)}");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching or saving weather data.");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }
        }
    }
}