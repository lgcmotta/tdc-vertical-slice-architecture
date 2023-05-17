using System;
using System.Diagnostics.CodeAnalysis;
using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioWarren.Infrastructure.EntityFramework.Configurations
{
    [ExcludeFromCodeCoverage]
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ConfigurePrimaryKey();

            builder.IgnoreDomainEvents();

            builder.Property(account => account.Name)
                .HasField("_name");

            builder.Property(account => account.Email)
                .HasField("_email");
            
            builder.Property(account => account.PhoneNumber)
                .HasField("_phoneNumber");

            builder.Property(account => account.Number)
                .HasField("_accountNumber")
                .HasValueGenerator<AccountNumberValueGenerator>();

            builder.Property(account => account.LastModified)
                .HasField("_lastModified");

            builder.Property<DateTime>("Created");
            
            builder.HasOne(typeof(AccountBalance), "_accountBalance")
                .WithOne()
                .HasForeignKey(typeof(AccountBalance), "AccountId")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}