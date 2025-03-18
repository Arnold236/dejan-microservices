
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder  app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {

                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("==> Applying migrations to Production Database.");
                try
                {
                   context.Database.Migrate(); 
                }
                catch (Exception ex)
                {
                    
                  Console.WriteLine($"--> Could not run migrations error occure: {ex.Message}");
                }
                
            }
            if (!context.Platforms.Any())
            {
                 Console.WriteLine("==> Seeding data ... ");

                 context.Platforms.AddRange( 
                    new Platform() {Name = ".Net", Publisher =  "Microsoft", Cost = "free"},
                    new Platform() {Name = "MYSQL", Publisher =  "Microsoft", Cost = "free"},
                    new Platform() {Name = "React", Publisher =  "Meta", Cost = "free"}
                 );
                 context.SaveChanges();
            }
            else{
                Console.WriteLine("==> We already have data! ");
            }
        }
    }
}