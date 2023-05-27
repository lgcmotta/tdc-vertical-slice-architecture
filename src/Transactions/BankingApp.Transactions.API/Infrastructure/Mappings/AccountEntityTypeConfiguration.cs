using BankingApp.Transactions.Domain;
using BankingApp.Transactions.Domain.Entities;
using BankingApp.Transactions.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Transactions.API.Infrastructure.Mappings;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(account => account.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(account => account.Currency)
            .HasConversion(
                currency => currency.Key,
                key => Currency.ParseByKey<Currency>(key)
            )
            .IsRequired();

        builder.Property(account => account.Balance)
            .HasConversion(
                balance => balance.Value,
                balance => new Money(balance)
            )
            .HasPrecision(19, 4)
            .IsRequired();

        builder.Property(account => account.CreatedAt)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
            .IsRequired();

        builder.Property(account => account.ModifiedAt)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
            .IsRequired(false);

        builder.HasMany(account => account.Transactions)
            .WithOne()
            .HasForeignKey("AccountId")
            .OnDelete(DeleteBehavior.NoAction);

        builder.Metadata.FindNavigation("Transactions")
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(account => account.Holder)
            .WithOne()
            .HasForeignKey(typeof(Holder), "AccountId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}