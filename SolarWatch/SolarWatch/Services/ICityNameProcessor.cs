namespace SolarWatch.Services;

public interface ICityNameProcessor
{
    public float GetLonCoord(string cityName);
    public float GetLatCoord(string cityName);
}