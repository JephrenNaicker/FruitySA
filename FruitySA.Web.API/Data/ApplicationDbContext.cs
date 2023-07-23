using FruitySA.Web.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FruitySA.Web.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}
