using BankingApp.Domain.Entities;
using BankingApp.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace BankingApp.Infrastructure.EntityFramework.Configurations;

[ExcludeFromCodeCoverage]
public class AccountBalanceEntityTypeConfiguration : IEntityTypeConfiguration<AccountBalance>
{
    public void Configure(EntityTypeBuilder<AccountBalance> builder)
    {
        builder.ConfigurePrimaryKey();

        builder.IgnoreDomainEvents();

        builder.Property(accountBalance => accountBalance.Balance)
            .HasField("_balance")
            .HasPrecision(19, 4);

        builder.Property(accountBalance => accountBalance.Transactions)
            .HasField("_transactions");

        builder.Property(accountBalance => accountBalance.Currency)
            .HasField("_currency")
            .HasConversion(currency => currency.Value
                , isoCode => Enumeration.GetItemByValue<Currency>(isoCode));

        builder.Ignore(accountBalance => accountBalance.Transactions);
            
        builder.HasMany(typeof(AccountTransaction), "_transactions")
            .WithOne()
            .HasForeignKey("AccountBalanceId")
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Property(accountBalance => accountBalance.LastModified)
            .HasField("_lastModified");
    }
}