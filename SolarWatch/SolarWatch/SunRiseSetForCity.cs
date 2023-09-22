using Microsoft.AspNetCore.Mvc;

namespace SolarWatch;

public class SunRiseSetForCity
{
    public string CityName { get; set; }
    public DateTime Date { get; set; }
    public DateTime SunSet { get; set; }
    public DateTime SunRise { get; set; }
}