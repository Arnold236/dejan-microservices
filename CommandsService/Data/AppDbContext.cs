using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
// using CommandsService.Data;

namespace CommandsService.Data
{
   public class AppDbContext : DbContext
   {
     public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
     {

     }

     public DbSet<Platform> Platforms {get; set;}
     public DbSet<Command> Commands {get; set;}

   //   protected ovveride void OnModelCreating(ModelBuilder modelBuilder)
   //   {
   //      modelBuilder
   //          .Entity<Platform>()
   //          .HasMany(p => p.Commands)
   //          .WithOne(p => p.Platform!)
   //          .HasForeignKey(p => p.PlatformId);

   //      modelBuilder
   //          .Entity<Command>()
   //          .HasMany(p => p.Platform)
   //          .WithOne(p => p.Commands)
   //          .HasForeignKey(p => p.PlatformId);
            
   //   }
   }
}