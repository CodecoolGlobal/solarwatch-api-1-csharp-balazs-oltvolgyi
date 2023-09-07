namespace SolarWatch.Services;

public interface ICityNameProcessor
{
    public (float, float) GetCoords(string cityName);
    public float GetLonCoord(string cityName);
    public float GetLatCoord(string cityName);
}