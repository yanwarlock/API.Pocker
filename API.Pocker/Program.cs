using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Pocker.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Pocker
{
    public class Program
    {
        /* public static void Main(string[] args)
         {
             CreateHostBuilder(args).Build().Run();
         }

         public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });*/

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await InitialActionsAsync();
            await host.RunAsync();
            async Task InitialActionsAsync()
            {
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var logger = services.GetService<ILogger<Program>>();

                await RunPendingMigrationsAsync();
                await CreateDefaultAdminAccountAsync();

                async Task RunPendingMigrationsAsync()
                {
                    try
                    {
                        var dbContext = services.GetService<ApplicationDbContext>();
                        if (dbContext != null)
                        {
                            var pendingMigration = await dbContext.Database.GetPendingMigrationsAsync();
                            if (pendingMigration.Any())
                            {
                                await dbContext.Database.MigrateAsync();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogWarning(e, $"Error running migrations");
                    }
                }
                async Task CreateDefaultAdminAccountAsync()
                {
                    var config = services.GetService<IConfiguration>();
                    var adminEmail = config["Admin:Email"];
                    var adminPassword = config["admin:Password"];

                    var usermanager = services.GetService<UserManager<IdentityUser>>();
                    var rolmanager = services.GetService<RoleManager<IdentityRole>>();

                    var user = await usermanager.FindByEmailAsync(adminEmail);

                    if (user is null)
                    {
                        var adminIdentityUser = new IdentityUser(adminEmail)
                        {
                            Email = adminEmail,
                        };

                        var createAdminIdentityUser = await usermanager.CreateAsync(adminIdentityUser, adminPassword);
                        if (!createAdminIdentityUser.Succeeded)
                        {
                            logger.LogWarning("Unable to create the admin");
                        }
                        //create rol
                        const string adminRoleName = "Admin";
                        var adminIndentityRole = new IdentityRole(adminRoleName);
                        var createAdminIndentityRole = await rolmanager.CreateAsync(adminIndentityRole);
                        if (!createAdminIndentityRole.Succeeded)
                        {
                            logger.LogWarning("Unable to create the rol");
                        }
                        const string basicyRoleName = "Basic";
                        var basicIndentityRole = new IdentityRole(basicyRoleName);
                        var createBasicIndentityRole = await rolmanager.CreateAsync(basicIndentityRole);

                        if (!createBasicIndentityRole.Succeeded)
                        {
                            logger.LogWarning("Unable to create the rol");
                        }
                        //assign rol to the user
                        var addToAndoRole = await usermanager.AddToRoleAsync(adminIdentityUser, adminRoleName);
                        if (!addToAndoRole.Succeeded)
                        {
                            logger.LogWarning($"Unable to add the admin user to '{adminRoleName}' role");
                        }
                    }


                }
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
