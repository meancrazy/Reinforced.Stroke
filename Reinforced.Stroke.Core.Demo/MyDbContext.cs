using Microsoft.EntityFrameworkCore;
using Reinforced.Stroke.Core.Demo.Data;

namespace Reinforced.Stroke.Core.Demo
{
    public class MyDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        public MyDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=strokes.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>().ToTable("Goods");
            builder.Entity<Customer>().Property(x => x.RegisterDate).HasColumnName("RegisteredAt");
            builder.Entity<Order>().Property(x => x.Subtotal).HasColumnName("Total");
        }
    }
}