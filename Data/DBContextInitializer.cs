#nullable disable
using BoardGameBrawl.Data.Models;
using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BoardGameBrawl.Data
{
    public static class DBContextInitializer
    {
        public async static Task Initialize(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserScheduleStore<UserSchedule, ApplicationUser> userScheduleStore)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();

            
            ApplicationUser admin = new()
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true
            };

            if (dbContext.Users.Any(u => u.UserName == "admin"))
            {
                return;
            }
            else
            {
                await userManager.CreateAsync(admin);
                await userManager.AddPasswordAsync(admin, "Admin123!");
            }

            if (dbContext.Roles.Any(r => r.Name == "Administrator"))
            {
                return;
            }
            else
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
                await roleManager.CreateAsync(new IdentityRole("Moderator"));
                await roleManager.CreateAsync(new IdentityRole("Host"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Adding all roles to Admin accouunt and creating claims for Admin account

            await userManager.AddToRoleAsync(admin, "Administrator");
            await userManager.AddToRoleAsync(admin, "Moderator");
            await userManager.AddToRoleAsync(admin, "Host");
            await userManager.AddToRoleAsync(admin, "User");

            var adminClaims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, admin.Id),
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Email, "admin@admin.com"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.Role, "Moderator"),
            new Claim(ClaimTypes.Role, "Host"),
            new Claim(ClaimTypes.Role, "User")
            };
            await userManager.AddClaimsAsync(admin, adminClaims);
           
            string filePath = "C:\\Users\\Tomson\\source\\repos\\BoardGameBrawl\\Resources\\user_data.csv";
            bool firstLine = true;

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }

                    string Username = values[0];
                    string FirstName = values[1];
                    string LastName = values[2];
                    string Email = values[3];
                    DateOnly creationDate = DateOnly.FromDateTime(DateTime.Now);

                    // Create a new instance of ApplicationUser
                    ApplicationUser entry = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = Username,
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        EmailConfirmed = true,
                        UserCreatedTime = creationDate,
                        UserLastLogin = creationDate
                    };
                    await userManager.CreateAsync(entry);
                    await userManager.AddPasswordAsync(entry, "Zaq1@WSX");
                    await userManager.AddToRoleAsync(entry, "User");

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, entry.Id),
                        new Claim(ClaimTypes.Name, entry.UserName),
                        new Claim(ClaimTypes.Email, entry.Email),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    await userManager.AddClaimsAsync(entry, userClaims);

                    UserSchedule userSchedule = new UserSchedule();
                    await userScheduleStore.SetUserByAsync(userSchedule, entry);
                    await userScheduleStore.CreateScheduleAsync(userSchedule);
                }
            }

            // save all changes to ApplicationDbContext
            await dbContext.SaveChangesAsync();
        }       
    }
}