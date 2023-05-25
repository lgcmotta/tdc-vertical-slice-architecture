namespace BankingApp.Domain.Core;

public interface IEntity<out TId>
{
    public TId Id { get; }
}