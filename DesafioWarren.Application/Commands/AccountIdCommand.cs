using System;
using DesafioWarren.Application.Models;
using MediatR;

namespace DesafioWarren.Application.Commands
{
    public class AccountIdCommand : IRequest<Response>
    {
        public Guid AccountId { get; set; }

        public AccountIdCommand(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}