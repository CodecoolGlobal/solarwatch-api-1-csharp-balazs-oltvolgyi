using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public class CityRepository : ICityRepository
{
    public IEnumerable<City> GetAll()
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Cities.ToList();
    }

    public City? GetByName(string name)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void Add(City city)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void Update(City city)
    {  
        using var dbContext = new SolarWatchContext();
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}