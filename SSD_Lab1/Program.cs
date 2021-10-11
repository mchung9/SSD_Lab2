using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SSD_Lab1.Data;
using SSD_Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Lab1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var configuration = host.Services.GetService<IConfiguration>();
            var hosting = host.Services.GetService<IWebHostEnvironment>();
            var secrets = configuration.GetSection("Secrets").Get<AppSecrets>();
            DbInitializer.appSecrets = secrets;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                //Seed users and roles
                DbInitializer.SeedUsersAndRoles(scope.ServiceProvider).Wait();
                //Seed teams if Teams database has not been seeded yet
                DbInitializer.SeedTeams(context, services);
                host.Run(); 
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
