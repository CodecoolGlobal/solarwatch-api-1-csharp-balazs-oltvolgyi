using System.Net;
using Newtonsoft.Json;

namespace SolarWatch.Services;

public class CityNameProcessor : ICityNameProcessor
{
    public async Task<float> GetLatCoord(string cityName)
    {
        var (lat, _) = await GetCoords(cityName);
        return lat;
    }

    public async Task<float> GetLonCoord(string cityName)
    {
        var (_, lon) = await GetCoords(cityName);
        return lon;
    }
    private async Task<(float, float)> GetCoords(string cityName)
    {
        
        var apiKey = "ec6e9277ce603085dd100a3df5d457fe";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";
        using (var client = new HttpClient())
        {
            var responseJson = await client.GetStringAsync(url);

            // Deserialize the JSON response into an array of objects
            var locations = JsonConvert.DeserializeObject<Location[]>(responseJson);

            if (locations.Length > 0)
            {
                var lat = locations[0].lat;
                var lon = locations[0].lon;
                return (lat, lon);
            }
            else
            {
                return (0, 0);
            }
        }
    }
}

public class Location
{
    public float lat { get; set; }
    public float lon { get; set; }
}


