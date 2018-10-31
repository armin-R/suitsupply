using System;
using Armin.Suitsupply.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Armin.Suitsupply.Domain.Data;
using Armin.Suitsupply.Web;

namespace IntergrationTests
{
    #region snippet1
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Startup>
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<DomainDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });


                services.AddDomain();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DomainDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        var seed = new DataSeed(db);
                        seed.Seed();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the " +
                            "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
    #endregion
}
