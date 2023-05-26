namespace BankingApp.Domain.Core;

public interface IModifiable
{
    void SetModificationDateTime(DateTime modifiedAt);
}