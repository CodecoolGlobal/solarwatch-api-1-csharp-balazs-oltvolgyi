using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Services;

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
    public void NotFoundCityName_ReturnsNull()
    {
        
        Assert.IsNull(_controller.Get("asdddsawer", DateTime.Now));
  
    }
    
    
}