using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Model;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<SunTimes> SunTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True");
    }
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //Configure the City entity - making the 'Name' unique
        builder.Entity<City>()
            .HasIndex(u => u.Name)
            .IsUnique();
        
        builder.Entity<SunTimes>()
            .HasIndex(u => u.SunSet)
            .IsUnique();
    

    }

}