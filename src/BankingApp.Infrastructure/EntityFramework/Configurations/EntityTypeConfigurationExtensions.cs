using BankingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace BankingApp.Infrastructure.EntityFramework.Configurations;

[ExcludeFromCodeCoverage]
public static class EntityTypeConfigurationExtensions
{
    public static EntityTypeBuilder<TEntity> IgnoreDomainEvents<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity
    {
        builder.Ignore(entity => entity.DomainEvents);

        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigurePrimaryKey<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
            .ValueGeneratedOnAdd();

        return builder;
    }
}