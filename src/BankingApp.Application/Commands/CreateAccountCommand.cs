using BankingApp.Application.Models;
using MediatR;

namespace BankingApp.Application.Commands;

public class CreateAccountCommand : IRequest<Response>
{
    public AccountModelBase Account { get; }

    public CreateAccountCommand(AccountModelBase account)
    {
        Account = account;
    }
}