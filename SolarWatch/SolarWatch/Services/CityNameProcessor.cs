using System.Net;
using Newtonsoft.Json;

namespace SolarWatch.Services;

public class CityNameProcessor : ICityNameProcessor
{
    public float GetLanCoord(string cityName)
    {
        return GetCoords(cityName).Item1;
    }
    
    public float GetLonCoord(string cityName)
    {
        return GetCoords(cityName).Item2;
    }
    public (float, float) GetCoords(string cityName)
    {
        
        var apiKey = "ec6e9277ce603085dd100a3df5d457fe";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";
        using (var client = new WebClient())
        {
            var responseJson = client.DownloadString(url);

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
                throw new InvalidOperationException("No locations found for the given city.");
            }
        }
    }
}

public class Location
{
    public float lat { get; set; }
    public float lon { get; set; }
    // Add other properties from the JSON response here
}


