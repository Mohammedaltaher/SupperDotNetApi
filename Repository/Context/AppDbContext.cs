
using Domain.Entities.Lookups;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        if (Database.IsSqlServer())
            Database.Migrate();
    }
    public DbSet<MainCategory> MainCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MainCategory>().HasKey(m => m.Id);
    }
}

