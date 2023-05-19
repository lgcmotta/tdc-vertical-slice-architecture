using BankingApp.Application.Models;
using System;

namespace BankingApp.Application.Commands;

public class UpdateAccountCommand : AccountIdCommand
{
    public AccountModelBase Account { get; set; }

    public UpdateAccountCommand(Guid accountId, AccountModelBase account) : base(accountId) 
    {
        Account = account;
    }
}