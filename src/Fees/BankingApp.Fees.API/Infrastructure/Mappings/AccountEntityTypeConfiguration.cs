﻿using BankingApp.Taxes.Domain;
using BankingApp.Taxes.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Fees.API.Infrastructure.Mappings;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(account => account.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(account => account.Token)
            .IsRequired();

        builder.Property(account => account.CurrentBalanceInUSD)
            .HasConversion(
                balance => balance.Value,
                balance => new Money(balance)
            )
            .IsRequired();

        builder.Property(account => account.LastBalanceChange)
            .IsRequired();

        builder.HasMany(account => account.FeeHistory)
            .WithOne()
            .HasForeignKey("AccountId")
            .OnDelete(DeleteBehavior.NoAction);
    }
}