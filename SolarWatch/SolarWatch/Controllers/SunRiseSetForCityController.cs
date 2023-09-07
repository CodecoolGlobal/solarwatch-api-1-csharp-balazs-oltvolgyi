using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;
[ApiController]
[Route("[controller]")]
public class SunRiseSetForCityController : ControllerBase
{
    private readonly ILogger<SunRiseSetForCityController> _logger;
    private readonly ICityNameProcessor _cityNameProcessor;

    public SunRiseSetForCityController(ILogger<SunRiseSetForCityController> logger, ICityNameProcessor cityNameProcessor)
    {
        _logger = logger;
        _cityNameProcessor = cityNameProcessor;
    }

    [HttpGet(Name = "GetSunRiseSetForCity")]
    public SunRiseSetForCity Get(string cityName, DateTime dateTime)
    {
        _logger.LogInformation(_cityNameProcessor.GetLanCoord(cityName).ToString());
        return new SunRiseSetForCity
        {
            CityName = cityName,
            Date = DateTime.Today,
            SunRise = DateTime.Now,
            SunSet = DateTime.MaxValue,
        };

    }
}