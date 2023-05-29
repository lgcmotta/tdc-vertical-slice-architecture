using BankingApp.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingApp.Infrastructure.Core.Interceptors;

public class CreatableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData is null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        var creatableEntries = eventData.Context?.ChangeTracker.Entries<ICreatableEntity>() ?? Enumerable.Empty<EntityEntry<ICreatableEntity>>();

        foreach (var creatableEntry in creatableEntries)
        {
            if (creatableEntry.State is not EntityState.Added)
            {
                continue;
            }

            creatableEntry.Entity.SetCreationDateTime(DateTime.UtcNow);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}