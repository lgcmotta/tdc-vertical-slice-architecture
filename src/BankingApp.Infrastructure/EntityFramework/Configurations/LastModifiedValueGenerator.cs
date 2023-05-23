using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace BankingApp.Infrastructure.EntityFramework.Configurations;

public class LastModifiedValueGenerator : ValueGenerator<DateTime>
{
    public override DateTime Next(EntityEntry entry)
    {
        return DateTime.Now;
    }

    public override bool GeneratesTemporaryValues { get; }
}