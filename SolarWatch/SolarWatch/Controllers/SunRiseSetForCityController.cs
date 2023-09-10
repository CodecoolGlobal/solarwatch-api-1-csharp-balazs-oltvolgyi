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
    public ActionResult<SunRiseSetForCity> Get(string cityName, DateTime date)
    {
        string formattedDate = date.ToString("yyyy'-'M'-'d");

        try
        {
            // getting coordinates based on city name
            var lat = _cityNameProcessor.GetLatCoord(cityName);
            var lon = _cityNameProcessor.GetLonCoord(cityName);
            _logger.LogInformation($"Data from _cityNameProcessor CITY:{cityName} --- LAT:{lat}, LON:{lon}");
            
            if(lat==0) 
            {
                _logger.LogError($"Error getting coordinates for city {cityName}");
                return StatusCode(500, $"Error getting coordinates for city {cityName}");
            }
            
            try
            {
                // using coordinates to get times of sunrise/set
                var sunrise = _coordAndDateProcessor.GetSunriseTime(lat, lon, formattedDate);
                var sunset = _coordAndDateProcessor.GetSunsetTime(lat, lon, formattedDate);
                _logger.LogInformation($"Data from _coordAndDateProcessor --- RISE:{sunrise}, SET:{sunset}");

                return Ok(new SunRiseSetForCity
                {
                    CityName = cityName,
                    Date = date,
                    SunRise = sunrise,
                    SunSet = sunset,
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting sunrise and sunset times for city {cityName} at date {formattedDate}");
                return StatusCode(500, $"Error getting sunrise and sunset times for city {cityName} at date {formattedDate}");

            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error getting coordinates for city {cityName}");
            return StatusCode(500, $"Error getting coordinates for city {cityName}");
        }
    }
}