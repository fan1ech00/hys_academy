using Microsoft.EntityFrameworkCore;
using webapi.Entities;
using webapi.Entities.Configurations;

namespace webapi;

public class AppContext: DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public AppContext(DbContextOptions<AppContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}