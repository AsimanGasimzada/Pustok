using Microsoft.EntityFrameworkCore;
using Pustok.Core.Entites;
using Pustok.Core.Entites.Common;
using Pustok.DataAccess.Interceptors;
using System.Reflection;

namespace Pustok.DataAccess.Contexts;

internal class AppDbContext(BaseAuditableInterceptor _interceptor, DbContextOptions options) : DbContext(options)
{


    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptor);
        base.OnConfiguring(optionsBuilder);
    }


    //public override int SaveChanges()
    //{
    //    var entries = this.ChangeTracker.Entries<BaseAuditableEntity>().ToList();

    //    foreach (var entry in entries)
    //    {
    //        switch (entry.State)
    //        {
    //            case EntityState.Added:
    //                entry.Entity.CreatedDate = DateTime.UtcNow;
    //                entry.Entity.CreatedBy = "Elchin";
    //                break;

    //            case EntityState.Modified:
    //                entry.Entity.UpdatedBy = "Elchin";
    //                entry.Entity.UpdatedDate = DateTime.UtcNow;
    //                break;

    //            case EntityState.Deleted:
    //                entry.Entity.DeletedBy = "Elchin";
    //                entry.Entity.DeletedDate = DateTime.UtcNow;
    //                entry.Entity.IsDeleted = true;
    //                entry.State = EntityState.Modified;
    //                break;
    //        }
    //    }

    //    return base.SaveChanges();
    //}

    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    var entries = this.ChangeTracker.Entries<BaseAuditableEntity>().ToList();

    //    foreach (var entry in entries)
    //    {
    //        switch (entry.State)
    //        {
    //            case EntityState.Added:
    //                entry.Entity.CreatedDate = DateTime.UtcNow;
    //                entry.Entity.CreatedBy = "Elchin";
    //                break;

    //            case EntityState.Modified:
    //                entry.Entity.UpdatedBy = "Elchin";
    //                entry.Entity.UpdatedDate = DateTime.UtcNow;
    //                break;

    //            case EntityState.Deleted:
    //                entry.Entity.DeletedBy = "Elchin";
    //                entry.Entity.DeletedDate = DateTime.UtcNow;
    //                entry.Entity.IsDeleted = true;
    //                entry.State = EntityState.Modified;
    //                break;
    //        }
    //    }


    //    return base.SaveChangesAsync(cancellationToken);
    //}   transfered to BaseAuditableInterceptor
}
