using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSD_Lab1.Models;
using System.Globalization;

namespace SSD_Lab1.Data
{
    public static class DbInitializer
    {
        public static AppSecrets appSecrets { get; set; }
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            //Migrate Database
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            //Get necessary component for seeding roles data
            var roleMgmt = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Get necessary component for seeding users data
            var userMgmt = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // If any roles exist do not populate database
            if (roleMgmt.Roles.Count() > 0)
                return 1;  

            // Seed roles
            int result = await SeedRoles(roleMgmt);
            if (result != 0)
                return 2;  

            // If there are any user exist do not populate database
            if (userMgmt.Users.Count() > 0)
                return 3;  

            // Seed users
            result = await SeedUsers(userMgmt);
            if (result != 0)
                return 4; 

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleMgmt)
        {
            // Create Manager Role which has read and write access to the Teams database
            var result = await roleMgmt.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;  

            // Create Player Role whihc has only read access to the Teams database
            result = await roleMgmt.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 2;  

            return 0;
        }
        
        

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userMgmt)
        {
            // Create a user with Manager role
            var manager = new ApplicationUser
            {
                UserName = "test.manager@testsite.org",
                Email = "test.manager@testsite.org",
                FirstName = "Test",
                LastName = "Manager",
                EmailConfirmed = true
            };
            //Set password for manager
            var result = await userMgmt.CreateAsync(manager, appSecrets.ManagerPass);
            if (!result.Succeeded)
                return 1;  

            // Assign Manager role
            result = await userMgmt.AddToRoleAsync(manager, "Manager");
            if (!result.Succeeded)
                return 2;  

            // Create a user with Player role
            var player = new ApplicationUser
            {
                UserName = "test.player@testsite.org",
                Email = "test.player@testsite.org",
                FirstName = "Test",
                LastName = "Player",
                EmailConfirmed = true
            };
            //Set a password
            result = await userMgmt.CreateAsync(player, appSecrets.PlayerPass);
            if (!result.Succeeded)
                return 3;  

            // Assign Player role
            result = await userMgmt.AddToRoleAsync(player, "Player");
            if (!result.Succeeded)
                return 4; 

            return 0;
        }
        //Seed Teams Database
        public static void SeedTeams(ApplicationDbContext context, IServiceProvider services)
        {
            //Make sure teams database is created
            context.Database.EnsureCreated();

            //If there are any data in Teams database, do not seed any data
            if (context.Teams.Any())
            {
                return;
            }

            //Three teams for example purposes
            var teamOne = new Team
            {
                TeamName = "Test Team",
                Email = "test.team@testsite.org",
                EstablishedDate = DateTime.ParseExact("2021-05-04", "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            var teamTwo = new Team
            {
                TeamName = "Azalea",
                Email = "azalea@testsite.org",
                EstablishedDate = DateTime.ParseExact("2021-03-08", "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            var teamThree = new Team
            {
                TeamName = "Sheriff FC",
                Email = "sheriff.fc@testsite.org",
                EstablishedDate = DateTime.ParseExact("2021-04-06", "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };

            //Add teams to database context
            context.Teams.Add(teamOne);
            context.Teams.Add(teamTwo);
            context.Teams.Add(teamThree);
            //Add teams to database
            context.SaveChanges();
        }
    }
}
