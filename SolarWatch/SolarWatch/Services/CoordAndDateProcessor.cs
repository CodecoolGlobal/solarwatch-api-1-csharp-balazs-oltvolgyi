using System.Net;
using System.Text.Json;
using Newtonsoft.Json;

namespace SolarWatch.Services;

public class CoordAndDateProcessor : ICoordAndDateProcessor
{
    public string GetSunriseTime(float lat, float lon, string date)
    {
        return GetSunRiseSetTime(lat, lon, date).Item1;
    }
    
    public string GetSunsetTime(float lat, float lon, string date)
    {
        return GetSunRiseSetTime(lat, lon, date).Item2;
    }

    private (string, string) GetSunRiseSetTime(float lat, float lon, string date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";
        using (var client = new WebClient())
        {
            var responseJson = client.DownloadString(url);

            if (responseJson is null) // inkább JSON-parse után checkolni a JSON-ban lévő response statust?
            {
                return (null, null);
            }
            
            JsonDocument json = JsonDocument.Parse(responseJson);


            JsonElement results = json.RootElement.GetProperty("results");

            string sunrise = results.GetProperty("sunrise").GetString();
            string sunset = results.GetProperty("sunset").GetString();

            
           return (sunrise, sunset);
        }

        
    }

    // private class RiseAndSetTimes
    // {
    //     public DateTime Sunrise { get; set; }
    //     public DateTime Sunset { get; set; }
    // }
}