using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Fees.API.Features.OverdraftFee;

public record OverdraftFeeCommand(decimal Rate) : IRequest, ICommand;