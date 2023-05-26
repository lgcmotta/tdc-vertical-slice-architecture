namespace BankingApp.Domain.Core;

public interface IModifiableEntity
{
    void SetModificationDateTime(DateTime modifiedAt);
}