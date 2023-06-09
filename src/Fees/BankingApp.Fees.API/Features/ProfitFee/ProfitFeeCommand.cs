﻿using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Fees.API.Features.ProfitFee;

public record ProfitFeeCommand(decimal Rate, int BalanceIdleInMinutes) : IRequest, ICommand;