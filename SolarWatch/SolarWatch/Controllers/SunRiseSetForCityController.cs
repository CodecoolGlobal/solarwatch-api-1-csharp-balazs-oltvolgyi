using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;
[ApiController]
[Route("[controller]")]
public class SunRiseSetForCityController : ControllerBase
{
    private readonly ILogger<SunRiseSetForCityController> _logger;
    private readonly ICityNameProcessor _cityNameProcessor;
    private readonly ICoordAndDateProcessor _coordAndDateProcessor;

    public SunRiseSetForCityController(ILogger<SunRiseSetForCityController> logger, ICityNameProcessor cityNameProcessor, ICoordAndDateProcessor coordAndDateProcessor)
    {
        _logger = logger;
        _cityNameProcessor = cityNameProcessor;
        _coordAndDateProcessor = coordAndDateProcessor;
    }

    [HttpGet(Name = "GetSunRiseSetForCity")]
    public SunRiseSetForCity Get(string cityName, DateTime date)
    {
        string formattedDate = date.ToString("yyyy'-'M'-'d");
        
        // getting coordinates based on city name
        var lat = _cityNameProcessor.GetLatCoord(cityName);
        var lon = _cityNameProcessor.GetLonCoord(cityName);
        
        _logger.LogInformation($"Data from _cityNameProcessor --- LAT:{lat}, LON:{lon}");
        
        // using coordinates to get times of sunrise/set
        var sunrise = _coordAndDateProcessor.GetSunriseTime(lat, lon, formattedDate);
        var sunset = _coordAndDateProcessor.GetSunsetTime(lat, lon, formattedDate);
        
        _logger.LogInformation($"Data from _coordAndDateProcessor --- RISE:{sunrise}, SET:{sunset}");
        
        return new SunRiseSetForCity
        {
            CityName = cityName,
            Date = date,
            SunRise = sunrise,
            SunSet = sunset,
        };

    }
}