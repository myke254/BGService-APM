using BGService_APM.BackgroundService;
using BGService_APM.Business;
using BGService_APM.DataAccess;
using BGService_APM.DataAccess.cache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<WeatherApiService>();
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddSingleton<ICacheManager, CacheManager>();
builder.Services.AddHostedService<WeatherWorkerService>();
builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
