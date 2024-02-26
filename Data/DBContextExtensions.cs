using BoardGameBrawl.Data.Models.Entities;
using BoardGameBrawl.Data.Stores.Implementations;
using BoardGameBrawl.Data.Stores.Interfaces;
using BoardGameBrawl.Services;
using Microsoft.AspNetCore.Identity;

namespace BoardGameBrawl.Data
{
    public static class DBContextExtensions
    {
        public static async Task<IApplicationBuilder> SeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var boardgameService = services.GetRequiredService<IBoardGameStore<BoardgameModel>>();
                var userScheduleService = services.GetRequiredService<IUserScheduleStore<UserSchedule, ApplicationUser>>();

                await DBContextInitializer.Initialize(dbContext, userManager, roleManager, userScheduleService);
                await BGGDBInfoLoader.LoadFromDB(dbContext, boardgameService);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during the process of seeding sql database: " + ex.Message);
            }
            return app;
        }
    }
}
