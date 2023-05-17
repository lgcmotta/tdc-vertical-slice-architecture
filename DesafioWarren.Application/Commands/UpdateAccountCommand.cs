using System;
using DesafioWarren.Application.Models;

namespace DesafioWarren.Application.Commands
{
    public class UpdateAccountCommand : AccountIdCommand
    {
        public AccountModelBase Account { get; set; }

        public UpdateAccountCommand(Guid accountId, AccountModelBase account) : base(accountId) 
        {
            Account = account;
        }
    }
}