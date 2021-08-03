using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication;
using Xunit;

namespace TestProject1
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public UnitTest1(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    var projectDir = Directory.GetCurrentDirectory();
                    var configPath = Path.Combine(projectDir, "testsettings.json");
                    conf.AddJsonFile(configPath);
                });
            });
        }
        
        [Fact]
        public async Task Test1()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<BearerHttpMessageHandler>();
            var client = _factory.CreateDefaultClient(handler);

            var response = await client.GetAsync("https://localhost:5001/weatherforecast");
            response.EnsureSuccessStatusCode();
        }
    }
}