namespace SolarWatch.Services;

public interface ICoordAndDateProcessor
{
    public string GetSunriseTime(float lat, float lon, string date);
    public string GetSunsetTime(float lat, float lon, string date);
}