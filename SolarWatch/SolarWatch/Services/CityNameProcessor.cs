using System.Net;
using Newtonsoft.Json;

namespace SolarWatch.Services;

public class CityNameProcessor : ICityNameProcessor
{
    public async Task<(double, double)> GetCoords(string cityName)
    {
        var lat = await GetLatCoord(cityName);
        var lon = await GetLonCoord(cityName);
        
        // if (lat == 0 || lon == 0) {return ERROR MESSAGE;} else {return (lat, lon);}
        
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
        string apiKey = config["ApiSettings:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("API key not found in configuration.");
        }
        else
        {
            Console.WriteLine($"API key found: {apiKey}");
        }
        
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
                var state = locations[0].state;
                var country = locations[0].country;

                Console.WriteLine($"FROM CityNameProcessor: LAT:{lat}");
                
                return (lat, lon, state, country);
            }
            else
            {
                return (0, 0, null, null);
            }
        }
    }
}

public class Location
{
    public double lat { get; set; }
    public double lon { get; set; }
    public string? state { get; set; }
    public string country { get; set; }
}


