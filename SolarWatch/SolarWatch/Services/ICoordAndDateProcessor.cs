namespace SolarWatch.Services;

public interface ICoordAndDateProcessor
{
    public Task<string> GetSunriseTime(double lat, double lon, string date);
    public Task<string> GetSunsetTime(double lat, double lon, string date);
}