using SolarWatch.Model;
using SolarWatch.Services;
using SolarWatch.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// registering interface(s) and their implementations in the DI container
builder.Services.AddSingleton<ICityNameProcessor, CityNameProcessor>();
builder.Services.AddSingleton<ICoordAndDateProcessor, CoordAndDateProcessor>();
builder.Services.AddSingleton<ICityRepository, CityRepository>();
builder.Services.AddSingleton<ISunTimesRepository, SunTimesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// void InitializeDb()
// {
//     using var db = new SolarWatchContext();
//     PrintCities();
//
//     void PrintCities()
//     {
//         Console.WriteLine("CITIES PRESENT IN DB:");
//         foreach (var city in db.Cities)
//         {
//             Console.WriteLine($" {city.Name}, {city.Latitude}, {city.Longitude}");
//         }
//     }
// }
//
// InitializeDb();

app.Run();