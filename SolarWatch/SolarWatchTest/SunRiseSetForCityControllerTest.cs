using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Services;
using NUnit;

namespace SolarWatchTest;

public class Tests
{
    private Mock<ILogger<SunRiseSetForCityController>> _loggerMock;
    private Mock<ICityNameProcessor> _cityNameProcessor;
    private Mock<ICoordAndDateProcessor> _coordAndDateProcessor;
    private SunRiseSetForCityController _controller;
    
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<SunRiseSetForCityController>>();
        _cityNameProcessor = new Mock<ICityNameProcessor>();
        _coordAndDateProcessor = new Mock<ICoordAndDateProcessor>();
        _controller = new SunRiseSetForCityController(_loggerMock.Object, _cityNameProcessor.Object, _coordAndDateProcessor.Object);
    }
    
    [Test]
    public async Task Get_ReturnsOkResult_WithValidData()
    {
        // Arrange
        var cityName = "Budapest";
        var date = DateTime.Parse("2022-12-12");

        float lat = 47.497993f;
        float lon = 19.04036f;

        var sunrise = "6:18:37 AM";
        var sunset = "2:56:42 PM";
        

        _cityNameProcessor.Setup(x => x.GetLatCoord(cityName)).ReturnsAsync(lat);
        _cityNameProcessor.Setup(x => x.GetLonCoord(cityName)).ReturnsAsync(lon);

        _coordAndDateProcessor.Setup(x => x.GetSunriseTime(lat, lon, date.ToString("yyyy'-'M'-'d"))).ReturnsAsync(sunrise);
        _coordAndDateProcessor.Setup(x => x.GetSunsetTime(lat, lon, date.ToString("yyyy'-'M'-'d"))).ReturnsAsync(sunset);

        // Act
        var result = await _controller.Get(cityName, date);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<OkObjectResult>(result.Result);
    }
    
    [Test]
    public async Task Get_ReturnsNotFound_WithIncorrectCityName()
    {
        // Arrange
        var cityName = "WrongCityName";
        var date = DateTime.Parse("2022-12-12");
        
        _cityNameProcessor.Setup(x => x.GetLatCoord(cityName)).ReturnsAsync(0); 
        _cityNameProcessor.Setup(x => x.GetLonCoord(cityName)).ReturnsAsync(0); 

       // Act
        var result = await _controller.Get(cityName, date);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.That(500, Is.EqualTo(objectResult.StatusCode));
        
    }
}