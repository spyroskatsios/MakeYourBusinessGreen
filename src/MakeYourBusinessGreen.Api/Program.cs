using MakeYourBusinessGreen.Infrastructure.Persistence;

namespace MakeYourBusinessGreen.Api;

public class Programm
{

    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var seedData = host.Services
               .CreateScope()
               .ServiceProvider
               .GetRequiredService<SeedData>();

        await seedData.SetUpRoles();
        await seedData.CreateAdminsAsync();

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
                  Host.CreateDefaultBuilder(args)
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.UseStartup<Startup>();
                      });
}