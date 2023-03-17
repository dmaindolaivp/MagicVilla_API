using MagicVilla_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace MagicVilla_Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        Id = 1,
                        Name = "Royal Villa",
                        Details = "A very very very very very very very very Royal Villa",
                        ImageUrl = "",
                        Rate = 200,
                        Amenity = ""
                    });
        }
    }
}
