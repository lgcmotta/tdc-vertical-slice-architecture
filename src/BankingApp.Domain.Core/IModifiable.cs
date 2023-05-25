namespace BankingApp.Domain.Core;

public interface IModifiable
{
    void LastModifiedAt(DateTime modifiedAt);
}