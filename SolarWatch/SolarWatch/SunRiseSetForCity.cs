using Microsoft.AspNetCore.Mvc;

namespace SolarWatch;

public class SunRiseSetForCity
{
    public string CityName { get; set; }
    public DateTime Date { get; set; }
    public string SunSet { get; set; }
    public string SunRise { get; set; }
}