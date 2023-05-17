using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DesafioWarren.Infrastructure.EntityFramework.Configurations
{
    public class LastModifiedValueGenerator : ValueGenerator<DateTime>
    {
        public override DateTime Next(EntityEntry entry)
        {
            return DateTime.Now;
        }

        public override bool GeneratesTemporaryValues { get; }
    }
}