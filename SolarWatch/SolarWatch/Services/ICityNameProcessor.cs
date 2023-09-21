namespace SolarWatch.Services;

public interface ICityNameProcessor
{
    public Task<float> GetLatCoord(string cityName);
    public Task<float> GetLonCoord(string cityName);
}