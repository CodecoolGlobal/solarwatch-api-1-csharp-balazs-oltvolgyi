using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;
[ApiController]
[Route("[controller]")]
public class SunRiseSetForCityController : ControllerBase
{
    private readonly ILogger<SunRiseSetForCityController> _logger;

    public SunRiseSetForCityController(ILogger<SunRiseSetForCityController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetSunRiseSetForCity")]
    public SunRiseSetForCity Get(string cityName)

    {
        return new SunRiseSetForCity
        {
            CityName = cityName,
            Date = DateTime.Today,
            SunRise = DateTime.Now,
            SunSet = DateTime.MaxValue,
        };

    }
}