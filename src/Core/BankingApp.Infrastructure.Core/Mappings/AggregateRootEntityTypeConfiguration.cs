using BankingApp.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Infrastructure.Core.Mappings;

public class AggregateRootEntityTypeConfiguration<TId> : IEntityTypeConfiguration<AggregateRoot<TId>>
{
    public void Configure(EntityTypeBuilder<AggregateRoot<TId>> builder)
    {
        builder.Ignore(aggregate => aggregate.DomainEvents);
    }
}