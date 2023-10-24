namespace SolarWatch.Services;

public interface ICityNameProcessor
{
    public Task<(double, double)> GetCoords(string cityName);
    public Task<double> GetLatCoord(string cityName);
    public Task<double> GetLonCoord(string cityName);
    public Task<string> GetCountry(string cityName);
    public Task<string> GetState(string cityName);
}