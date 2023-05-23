using BankingApp.Application.Models;
using MediatR;
using System;

namespace BankingApp.Application.Commands;

public class AccountIdCommand : IRequest<Response>
{
    public Guid AccountId { get; set; }

    public AccountIdCommand(Guid accountId)
    {
        AccountId = accountId;
    }
}