using SolarWatch.Contracts;
using SolarWatch.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using SolarWatch.Data;
using SolarWatch.Services.Repository;
using Newtonsoft.Json;
using System.Net;
using SolarWatch;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntegrationTest
{
    public class Tests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            // Create an instance of WebApplicationFactory for your ASP.NET application.
            var factory = new WebApplicationFactory<Program>();
            string connectionString = "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=true;";
            Environment.SetEnvironmentVariable("CONNECTION_STRING", connectionString);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

           
            _client = factory.CreateClient();

            AuthRequest authRequest = new AuthRequest("admin@admin.com", "admin123");
            var jsonString = JsonSerializer.Serialize(authRequest);
            var jsonStringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync("/Auth/Login", jsonStringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var desContent = JsonSerializer.Deserialize<AuthResponse>(content,options);
            var token = desContent.Token;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        [Test]
        public async Task GetSunriseSunsetAsync_ReturnsSunriseSunsetData()
        {
            // Arrange
            
            var cityName = "Budapest";
            var date = "2022-01-01";

            // Act
            var response = await _client.GetAsync($"SunRiseSetForCity?cityName={cityName}&date={date}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var sunriseSunsetData = JsonConvert.DeserializeObject<SunRiseSetForCity>(responseContent);
            
            Assert.NotNull(sunriseSunsetData);
            Assert.AreEqual(cityName, sunriseSunsetData.CityName); 
            //Assert.That(date, Is.EqualTo(sunriseSunsetData.Date)); 
        }
        
        [Test]
        public async Task GetSunriseSunsetAsync_ReturnsNotFoundForInvalidCity()
        {
            // Arrange
            var cityName = "NonExistentCity";
            var date = "2022-01-01";

            // Act
            var response = await _client.GetAsync($"SunRiseSetForCity?cityName={cityName}&date={date}");

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            // Add more assertions to ensure the response is as expected for invalid data.
        }
        
        [Test]
        public async Task CreateCity_ReturnsCreatedResponse()
        {
            // Arrange
            var newCity = new City
            {
                Name = "NewCity",
                Country = "Country",
                State = "State",
                Latitude = 42.1234,
                Longitude = -78.5678
            };
    
            var jsonString = JsonSerializer.Serialize(newCity);
            var jsonStringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/SunRiseSetForCity", jsonStringContent);

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }


    }
}