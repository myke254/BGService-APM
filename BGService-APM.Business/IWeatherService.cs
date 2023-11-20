using BGService_APM.DataAccess.models;

namespace BGService_APM.Business
{
    public interface IWeatherService
    {
        public Task<OpenWeatherMapResponse> GetWeatherData(string cityName);
    }
}
