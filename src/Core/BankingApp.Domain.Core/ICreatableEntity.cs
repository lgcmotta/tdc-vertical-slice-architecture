namespace BankingApp.Domain.Core;

public interface ICreatableEntity
{
    void SetCreationDateTime(DateTime createdAt);
}