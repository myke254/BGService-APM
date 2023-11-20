using BGService_APM.DataAccess;
using BGService_APM.DataAccess.models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BGService_APM.BackgroundService
{

    public class WeatherWorkerService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<WeatherWorkerService> _logger;
        private readonly WeatherApiService _weatherApiService;

        public WeatherWorkerService(
            ILogger<WeatherWorkerService> logger,
            WeatherApiService weatherApiService
            )
        {
            _logger = logger;
            _weatherApiService = weatherApiService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string cityName = "Nairobi";
                    OpenWeatherMapResponse weatherData = await _weatherApiService.GetWeatherDataAsync(cityName);
                    _logger.LogInformation($"Weather Data:: {JsonConvert.SerializeObject(weatherData)}");
                   // Wait for 5 minutes before fetching again
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