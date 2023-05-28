using BankingApp.Transactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Transactions.API.Infrastructure.Mappings;

public class HolderEntityTypeConfiguration : IEntityTypeConfiguration<Holder>
{
    public void Configure(EntityTypeBuilder<Holder> builder)
    {
        builder.ToTable("Holders");

        builder.Property(holder => holder.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(holder => holder.Name)
            .IsRequired();

        builder.Property(holder => holder.Document)
            .IsRequired();

        builder.Property(holder => holder.Token)
            .IsRequired();

        builder.Property(holder => holder.CreatedAt)
            .IsRequired();

        builder.Property(holder => holder.ModifiedAt)
            .IsRequired(false);
    }
}