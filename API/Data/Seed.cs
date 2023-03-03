using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {



        // UserManager<AppUser> userManager,
        //          RoleManager<AppRole> roleManager
        public static async Task SeedUsers(UserManager<AppUser> userManager,  RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

                var roles = new List<AppRole>{
               new AppRole{Name="Member"},
               new AppRole{Name="Admin"},
               new AppRole{Name="Moderator"}};

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                 Console.WriteLine("Hamzeh Ghabahshneh😊❤");
              
                user.UserName = user.UserName.ToLower();
                // user.passwordSalt = hmac.Key;
                // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
               // await userManager.GetSecurityStampAsync(user);

                //await userManager.UpdateSecurityStampAsync(user);

                // userManager.makeSecurity();
                 await userManager.CreateAsync(user, "Pa$$w0rd");
    
                await userManager.AddToRoleAsync(user, "Member");
                //  await context.Users.AddAsync(user);


            }
            // await userManager.SaveChangesAsync();

          //  await context.SaveChangesAsync();

            var admin = new AppUser
            {

                UserName = "admin",
            };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            // await userManager.UpdateSecurityStampAsync(admin);

            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

        }
    }
}