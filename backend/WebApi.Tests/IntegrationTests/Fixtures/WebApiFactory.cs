using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;

namespace WebApi.Tests.IntegrationTests.Fixtures;

public class WebApiFactory : WebApplicationFactory<Program>
{
    private static readonly string DbName = $"TestDb_{Guid.NewGuid():N}";
    private static bool _initialized = false;
    private static readonly object _lock = new object();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<NC07WebAppContext>));
            
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<NC07WebAppContext>(options =>
            {
                options.UseInMemoryDatabase(DbName);
            }, ServiceLifetime.Scoped);

            lock (_lock)
            {
                if (!_initialized)
                {
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<NC07WebAppContext>();
                    db.Database.EnsureCreated();
                    _initialized = true;
                }
            }
        });

        builder.UseEnvironment("Development");
    }
}
