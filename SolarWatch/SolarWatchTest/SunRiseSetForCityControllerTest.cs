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
    
    // [Test]
    // public void Get_ReturnsOkResult_WithValidData()
    // {
    //     // Arrange
    //     var cityName = "Budapest";
    //     var date = DateTime.Parse("2022-12-12");
    //     
    //     float lat = 47.497993f;
    //     float lon = 19.04036f;
    //     
    //     var sunrise = "6:18:37 AM";
    //     var sunset = "2:56:42 PM";
    //    
    //     _cityNameProcessor.Setup(x => x.GetLatCoord(cityName)).Returns(lat);
    //     _cityNameProcessor.Setup(x => x.GetLonCoord(cityName)).Returns(lon);
    //
    //     _coordAndDateProcessor.Setup(x => x.GetSunriseTime(lat, lon, date.ToString("yyyy'-'M'-'d"))).Returns(sunrise);
    //     _coordAndDateProcessor.Setup(x => x.GetSunsetTime(lat, lon, date.ToString("yyyy'-'M'-'d"))).Returns(sunset);
    //
    //     // Act
    //     var result = _controller.Get(cityName, date);
    //     var resultObject = result.Value;
    //     
    //     Assert.NotNull(result);
    //     Assert.IsInstanceOf<OkObjectResult>(result.Result);
    //
    //     Assert.IsInstanceOf<SunRiseSetForCity>(result.Value);        
    //     Assert.That(result.Value is SunRiseSetForCity);
    //     Assert.That(cityName.Equals(resultObject.CityName));
    //
    //     //Assert.That(result.Value is SunRiseSetForCity);
    //     Assert.That(cityName, Is.EqualTo(resultObject.CityName));
    //     
    //     Assert.That(cityName.Equals(result.Value.CityName));
    //     Assert.That(date.Equals(resultObject.Date));
    //     Assert.That(sunrise.Equals(resultObject.SunRise));
    //     Assert.That(sunset.Equals(resultObject.SunSet));
    // }

    [Test]
    public void Get_ReturnsOkResult_WithValidData()
    {
        // Arrange
        var cityName = "Budapest";
        var date = DateTime.Parse("2022-12-12");

        float lat = 47.497993f;
        float lon = 19.04036f;

        var sunrise = "6:18:37 AM";
        var sunset = "2:56:42 PM";
        

        _cityNameProcessor.Setup(x => x.GetLatCoord(cityName)).Returns(lat);
        _cityNameProcessor.Setup(x => x.GetLonCoord(cityName)).Returns(lon);

        _coordAndDateProcessor.Setup(x => x.GetSunriseTime(lat, lon, date.ToString("yyyy'-'M'-'d"))).Returns(sunrise);
        _coordAndDateProcessor.Setup(x => x.GetSunsetTime(lat, lon, date.ToString("yyyy'-'M'-'d"))).Returns(sunset);

        // Act
        var result = _controller.Get(cityName, date);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<OkObjectResult>(result.Result);
        //Assert.IsAssignableFrom<SunRiseSetForCity>(result.Value);
    }
    
    [Test]
    public void IncorrectCityName_ReturnsStatusCode500()
    {
        
    }
}