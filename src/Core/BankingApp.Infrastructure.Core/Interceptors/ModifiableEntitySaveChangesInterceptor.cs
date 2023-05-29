using BankingApp.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BankingApp.Infrastructure.Core.Interceptors;

public class ModifiableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData is null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        var modifiableEntries = eventData.Context?.ChangeTracker.Entries<IModifiableEntity>() ?? Enumerable.Empty<EntityEntry<IModifiableEntity>>();

        foreach (var modifiableEntry in modifiableEntries)
        {
            if (modifiableEntry.State is not (EntityState.Detached or EntityState.Modified))
            {
                continue;
            }

            modifiableEntry.Entity.SetModificationDateTime(DateTime.UtcNow);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);
    }
}