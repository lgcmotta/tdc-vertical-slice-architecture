using BankingApp.Transactions.Domain.Entities;
using BankingApp.Transactions.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Transactions.API.Infrastructure.Mappings;

public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(transaction => transaction.Sender)
            .IsRequired();

        builder.Property(transaction => transaction.Receiver)
            .IsRequired();

        builder.Property(transaction => transaction.Type)
            .HasConversion(
                type => type.Key,
                key => TransactionType.ParseByKey<TransactionType>(key)
            )
            .IsRequired();

        builder.Property(transaction => transaction.Occurence)
            .IsRequired();

        builder.Property<Money>("_value")
            .HasConversion(
                value => value.Value,
                value => new Money(value)
            )
            .HasColumnName("Value")
            .HasPrecision(19, 4)
            .IsRequired();

        builder.Property<Money>("_balanceSnapShot")
            .HasConversion(
                balance => balance.Value,
                balance => new Money(balance)
            )
            .HasColumnName("BalanceSnapShot")
            .HasPrecision(19, 4)
            .IsRequired();
    }
}