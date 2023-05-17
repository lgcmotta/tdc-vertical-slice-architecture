using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DesafioWarren.Infrastructure.EntityFramework.Configurations
{
    public class AccountNumberValueGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues { get; }

        public override string Next(EntityEntry entry)
        {
            return new Random().Next(10000, 9999999).ToString();
        }
        
    }
}