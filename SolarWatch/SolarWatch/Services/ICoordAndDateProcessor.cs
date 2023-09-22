namespace SolarWatch.Services;

public interface ICoordAndDateProcessor
{
    public Task<string> GetSunriseTime(float lat, float lon, string date);
    public Task<string> GetSunsetTime(float lat, float lon, string date);
}