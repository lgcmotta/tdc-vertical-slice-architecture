using BankingApp.Accounts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankingApp.Accounts.API.Infrastructure.Mappings;

public class AccountTokenEntityTypeConfiguration : IEntityTypeConfiguration<AccountToken>
{
    public void Configure(EntityTypeBuilder<AccountToken> builder)
    {
        builder.Property(token => token.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(token => token.Value)
            .IsRequired();

        builder.Property(token => token.CreatedAt)
            .IsRequired();

        builder.Property(token => token.Enabled)
            .IsRequired();

        builder.Property(token => token.DisabledAt)
            .IsRequired(false);
    }
}