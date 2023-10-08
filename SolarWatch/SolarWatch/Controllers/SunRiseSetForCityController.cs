using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Services;
using SolarWatch.Services.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunRiseSetForCityController : ControllerBase
{
    private readonly ILogger<SunRiseSetForCityController> _logger;
    private readonly ICityNameProcessor _cityNameProcessor;
    private readonly ICoordAndDateProcessor _coordAndDateProcessor;
    private readonly ICityRepository _cityRepository;
    private readonly ISunTimesRepository _sunTimesRepository;

    public SunRiseSetForCityController(ILogger<SunRiseSetForCityController> logger,
        ICityNameProcessor cityNameProcessor, ICoordAndDateProcessor coordAndDateProcessor,
        ICityRepository cityRepository, ISunTimesRepository sunTimesRepository)
    {
        _logger = logger;
        _cityNameProcessor = cityNameProcessor;
        _coordAndDateProcessor = coordAndDateProcessor;
        _cityRepository = cityRepository;
        _sunTimesRepository = sunTimesRepository;
    }

    [HttpGet(Name = "GetSunRiseSetForCity"), Authorize]
    public async Task<ActionResult<SunRiseSetForCity>> Get(string cityName, DateTime date)
    {
        string formattedDate = date.ToString("yyyy'-'M'-'d");
        double lat = 0;
        double lon = 0;
        
        var city = _cityRepository.GetByName(cityName);

        if (city == null)
        {
            _logger.LogInformation("City not yet present in DB, looking up info in API");

            try
            {
                // FROM API: getting coordinates based on city name
                lat = await _cityNameProcessor.GetLatCoord(cityName);
                lon = await _cityNameProcessor.GetLonCoord(cityName);

                if (lat == 0)
                {
                    _logger.LogError($"Error getting coordinates for city {cityName}");
                    return StatusCode(500, $"Error getting coordinates for city {cityName}");
                }

                var state = await _cityNameProcessor.GetState(cityName);
                var country = await _cityNameProcessor.GetCountry(cityName);

                _logger.LogInformation
                    ($"Data from _cityNameProcessor CITY:{cityName} --- LAT:{lat}, LON:{lon}, STATE:{state}, COUNTRY:{country}");

                // SAVING CITY data to DB
                _cityRepository.Add(new City()
                {
                    Name = cityName,
                    Country = country,
                    State = state,
                    Latitude = lat,
                    Longitude = lon,

                });
                _logger.LogInformation($"NEW INFO ADDED TO DB --- city:{cityName}, lat:{lat}, lon:{lon}, state:{state}, country:{country}");

                // GETTING SUNTIMES from API
                try
                {
                    var (sunrise, sunset) = await GetRiseAndSetWithDateType(lat, lon, formattedDate);
                    
                    // SAVING SUNTIMES data to DB
                    _sunTimesRepository.Add(new SunTimes()
                    {
                        CityName = cityName,
                        Date = date,
                        SunRise = sunrise,
                        SunSet = sunset
                    });
                    _logger.LogInformation($"NEW INFO ADDED TO DB --- city:{cityName}, date:{date}, rise:{sunrise}, set:{sunset}");

                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        $"Error getting sunrise and sunset times for city {cityName} at date {formattedDate}");
                    return NotFound(
                        $"Error getting sunrise and sunset times for city {cityName} at date {formattedDate}");
                }

            }

            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting coordinates for city {cityName}");
                return StatusCode(500, $"Error getting coordinates for city {cityName}");
            }
        }

        var SunTimesForDate = _sunTimesRepository.GetByDateAndName(cityName, date);
        if (SunTimesForDate == null)
        {
            var (sunrise, sunset) = await GetRiseAndSetWithDateType(lat, lon, formattedDate);
            
            // SAVING SUNTIMES data to DB
            _sunTimesRepository.Add(new SunTimes()
            {
                CityName = cityName,
                Date = date,
                SunRise = sunrise,
                SunSet = sunset
            });
            _logger.LogInformation($"NEW INFO ADDED TO DB --- city:{cityName}, date:{date}, rise:{sunrise}, set:{sunset}");
        }
        
        return Ok(new SunRiseSetForCity
        {
            CityName = cityName,
            Date = date,
            SunRise = _sunTimesRepository.GetByDateAndName(cityName,date).SunRise,
            SunSet = _sunTimesRepository.GetByDateAndName(cityName,date).SunSet,
        });

    }
    
    

    private async Task<(DateTime, DateTime)> GetRiseAndSetWithDateType(double lat, double lon, string formattedDate)
    {
        var sunriseTime = await _coordAndDateProcessor.GetSunriseTime(lat, lon, formattedDate);
        var sunsetTime = await _coordAndDateProcessor.GetSunsetTime(lat, lon, formattedDate);
        _logger.LogInformation(
            $"Data from _coordAndDateProcessor --- RISE:{sunriseTime}, SET:{sunsetTime}");
        
        // parsing to datetime format
        string timeFormat = "h:mm:ss tt";
        DateTime sunrise = DateTime.ParseExact(sunriseTime, timeFormat, CultureInfo.InvariantCulture);
        DateTime sunset = DateTime.ParseExact(sunsetTime, timeFormat, CultureInfo.InvariantCulture);
        Console.WriteLine($"FROM GETRISEANDSET: set:{sunset}");
        
        return (sunrise, sunset);
    }

    
    // CRUD endpoints for Cities and SunTimes
    [HttpPost, Authorize(Roles="Admin")]
    public ActionResult<City> CreateCity(City city)
    {
        _cityRepository.Add(city);
        return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
    }

    [HttpPost("/sunpost"), Authorize(Roles="Admin")]
    public ActionResult<SunTimes> CreateSunTimes(SunTimes sunTimes)
    {
        _sunTimesRepository.Add(sunTimes);
        return CreatedAtAction(nameof(GetSunTimes), new { id = sunTimes.Id }, sunTimes);
    }
    
    [HttpGet("{id}"), Authorize(Roles="User, Admin")]
    public ActionResult<City> GetCity(string name)
    {
        var city = _cityRepository.GetByName(name);
        if (city == null)
        {
            return NotFound();
        }
        return city;
    }

    [HttpGet("{cityName}/{date}"), Authorize(Roles="User, Admin")]
    public ActionResult<SunTimes> GetSunTimes(string cityName, DateTime date)
    {
        var sunTimes = _sunTimesRepository.GetByDateAndName(cityName, date);
        if (sunTimes == null)
        {
            return NotFound();
        }
        return sunTimes;
    }
    
    [HttpPut("{id}"), Authorize(Roles="Admin")]
    public IActionResult UpdateCity(int id, City city)
    {
        if (id != city.Id)
        {
            return BadRequest();
        }
        _cityRepository.Update(city);

        return NoContent();
    }

    [HttpPut("{cityName}/{date}"), Authorize(Roles="Admin")]
    public IActionResult UpdateSunTimes(string cityName, DateTime date, SunTimes sunTimes)
    {
        if (cityName != sunTimes.CityName || date != sunTimes.Date)
        {
            return BadRequest();
        }
        _sunTimesRepository.Update(sunTimes);

        return NoContent();
    }
    
    [HttpDelete("{id}"), Authorize(Roles="Admin")]
    public IActionResult DeleteCity(string name)
    {
        var city = _cityRepository.GetByName(name);
        if (city == null)
        {
            return NotFound();
        }
        _cityRepository.Delete(city);

        return NoContent();
    }

    [HttpDelete("{cityName}/{date}"), Authorize(Roles="Admin")]
    public IActionResult DeleteSunTimes(string cityName, DateTime date)
    {
        var sunTimes = _sunTimesRepository.GetByDateAndName(cityName, date);
        if (sunTimes == null)
        {
            return NotFound();
        }
        _sunTimesRepository.Delete(sunTimes);
        
        return NoContent();
    }
}