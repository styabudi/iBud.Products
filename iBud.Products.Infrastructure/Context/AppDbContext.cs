using iBud.Products.Infrastructure.Models.Common;
using iBud.Products.Infrastructure.Models.Tests;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iBud.Products.Infrastructure.Context;
public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<First> Firsts => Set<First>();

    private void AuditingProcess()
    {
        DateTime currentTime = DateTime.Now;
        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is BaseModel))
        {
            var entity = item.Entity as BaseModel;
            if (entity != null)
            {
                entity.CreatedDate = currentTime;
                entity.ModifiedDate = currentTime;
                entity.CreatedBy = "reachme@styabudi.com";
                entity.ModifiedBy = "reachme@styabudi.com";
                entity.IsActive = true;
            }

        }

        foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is BaseModel))
        {
            var entity = item.Entity as BaseModel;
            if (entity != null)
            {
                entity.ModifiedDate = currentTime;
                entity.ModifiedBy = "reachme@styabudi.com";
                item.Property(nameof(entity.CreatedDate)).IsModified = false;
                item.Property(nameof(entity.CreatedBy)).IsModified = false;
                entity.IsActive = entity.IsActive;
            }

        }
    }
}