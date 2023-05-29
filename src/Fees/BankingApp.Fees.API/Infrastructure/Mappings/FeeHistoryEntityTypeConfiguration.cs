﻿using BankingApp.Taxes.Domain.Entities;
using BankingApp.Taxes.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Fees.API.Infrastructure.Mappings;

public class FeeHistoryEntityTypeConfiguration : IEntityTypeConfiguration<FeeHistory>
{
    public void Configure(EntityTypeBuilder<FeeHistory> builder)
    {
        builder.Property(history => history.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(history => history.Amount)
            .HasConversion(
                balance => balance.Value,
                balance => new Money(balance)
            )
            .IsRequired();

        builder.Property(history => history.Type)
            .HasConversion(
                feeType => feeType.Key,
                key => FeeType.ParseByKey<FeeType>(key)
            )
            .IsRequired();

        builder.Property(history => history.CreatedAt)
            .IsRequired();
    }
}