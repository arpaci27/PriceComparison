using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PriceComparison.Models
{
    public class Context: DbContext
    {
        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-UT8GOU6;Database=XDB;integrated security =true;TrustServerCertificate=True;");
        }
    }
}
