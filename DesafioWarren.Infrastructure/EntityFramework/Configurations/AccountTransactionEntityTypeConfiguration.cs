using System.Diagnostics.CodeAnalysis;
using DesafioWarren.Domain.Entities;
using DesafioWarren.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioWarren.Infrastructure.EntityFramework.Configurations
{
    [ExcludeFromCodeCoverage]
    public class AccountTransactionEntityTypeConfiguration : IEntityTypeConfiguration<AccountTransaction>
    {
        public void Configure(EntityTypeBuilder<AccountTransaction> builder)
        {
            builder.ConfigurePrimaryKey();

            builder.IgnoreDomainEvents();

            builder.Property(accountTransaction => accountTransaction.TransactionType)
                .HasConversion(transactionType => transactionType.Id
                    , id => Enumeration.GetItemById<TransactionType>(id));

            builder.Property(accountTransaction => accountTransaction.TransactionValue)
                .HasField("_transactionValue")
                .HasPrecision(19, 4);

            builder.Property(accountTransaction => accountTransaction.BalanceBeforeTransaction)
                .HasField("_balanceBeforeTransaction")
                .HasPrecision(19, 4);

            builder.Property(accountTransaction => accountTransaction.Occurrence)
                .IsRequired();
        }
    }
}