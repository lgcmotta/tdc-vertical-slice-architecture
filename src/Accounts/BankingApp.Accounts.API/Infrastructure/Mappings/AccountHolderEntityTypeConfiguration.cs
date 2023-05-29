using BankingApp.Accounts.Domain;
using BankingApp.Accounts.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Accounts.API.Infrastructure.Mappings;

public class AccountHolderEntityTypeConfiguration : IEntityTypeConfiguration<AccountHolder>
{
    public void Configure(EntityTypeBuilder<AccountHolder> builder)
    {
        builder.Property(holder => holder.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(holder => holder.FirstName)
            .IsRequired();

        builder.Property(holder => holder.LastName)
            .IsRequired();

        builder.Property(holder => holder.Document)
            .IsRequired();

        builder.Property(holder => holder.FirstName)
            .IsRequired();

        builder.Property(holder => holder.Currency)
            .HasConversion(
                currency => currency.Key,
                key => Currency.ParseByKey<Currency>(key)
            )
            .IsRequired();

        builder.Property(holder => holder.CreatedAt)
            .IsRequired();

        builder.Property(holder => holder.ModifiedAt)
            .IsRequired(false);

        builder.HasMany(holder => holder.Tokens)
            .WithOne()
            .HasForeignKey("AccountId")
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Metadata.FindNavigation("Tokens")
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}