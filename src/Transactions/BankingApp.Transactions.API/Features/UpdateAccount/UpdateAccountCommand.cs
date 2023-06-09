﻿using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.UpdateAccount;

public record UpdateAccountCommand(Guid HolderId, string? Name, string? Token, string? Currency) : IRequest, ICommand;