namespace SolarWatch.Model;

public class SunTimes
{
    public int Id { get; init; }
    public string? CityName { get; set; }
    public DateTime SunRise { get; set; }
    public DateTime SunSet { get; set; }
    public DateTime Date { get; set; }
}