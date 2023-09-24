using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public interface ISunTimesRepository
{
    IEnumerable<SunTimes> GetAll();
    SunTimes? GetByCityName(string cityName);
    SunTimes? GetByDateAndName(string cityName, DateTime date);

    void Add(SunTimes sunTimes);
    void Delete(SunTimes sunTimes);
    void Update(SunTimes sunTimes);
}