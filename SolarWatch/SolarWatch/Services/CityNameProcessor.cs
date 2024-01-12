using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;

namespace SolarWatch.Services;

public class CityNameProcessor : ICityNameProcessor
{
    public async Task<(double, double)> GetCoords(string cityName)
    {
        var lat = await GetLatCoord(cityName);
        var lon = await GetLonCoord(cityName);
        
        return (lat, lon);
    }

    public async Task<double> GetLatCoord(string cityName)
    {
        var (lat, _, _, _) = await GetInfo(cityName);
        return lat;
    }

    public async Task<double> GetLonCoord(string cityName)
    {
        var (_, lon, _, _) = await GetInfo(cityName);
        return lon;
    }
    
    public async Task<string> GetState(string cityName)
    {
        var (_, _, state, _) = await GetInfo(cityName);
        return state;
    }
    
    public async Task<string> GetCountry(string cityName)
    {
        var (_, _, _, country) = await GetInfo(cityName);
        return country;
    }
    
    private async Task<(double, double, string?, string?)> GetInfo(string cityName)
    {
        
        // Building configuration from appsettings.json
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Getting the API key
        string? apiKey = config["ApiSettings:ApiKey"];
        Console.WriteLine(string.IsNullOrEmpty(apiKey)
            ? "API key not found in configuration."
            : $"API key found: {apiKey}");

        // Getting info from API with newly created URL and client
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";
        using var client = new HttpClient();
        var responseJson = await client.GetStringAsync(url);

        // Deserializing JSON response
        var locations = JsonConvert.DeserializeObject<Location[]>(responseJson);

        if (locations is { Length: > 0 })
        {
            var lat = locations[0].lat;
            var lon = locations[0].lon;
            var state = locations[0].state;
            var country = locations[0].country;
                
            return (lat, lon, state, country);
        }

        return (0, 0, null, null);
    }
}

public class Location
{
    public double lat { get; set; }
    public double lon { get; set; }
    public string? state { get; set; }
    public string country { get; set; }
}


