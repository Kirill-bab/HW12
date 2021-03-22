using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DepsWebApp.Models
{
    public class UserStorageContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserStorageContext(DbContextOptions<UserStorageContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.UseNpgsql("Host=localhost;Port=5435;Database=postgres;Username=postgres;Password=qwerty");
        }
    }
}
