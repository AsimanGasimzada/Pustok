using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pustok.Core.Entites.Common;
using Pustok.DataAccess.Contexts;

namespace Pustok.DataAccess.Interceptors;

internal class BaseAuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditColumns(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {

        UpdateAuditColumns(eventData);
        return base.SavingChanges(eventData, result);
    }

    private static void UpdateAuditColumns(DbContextEventData eventData)
    {
        if (eventData.Context is AppDbContext appDbContext)
        {
            var entries = appDbContext.ChangeTracker.Entries<BaseAuditableEntity>().ToList();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "Elchin";
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = "Elchin";
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.DeletedBy = "Elchin";
                        entry.Entity.DeletedDate = DateTime.UtcNow;
                        entry.Entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
    }

}
