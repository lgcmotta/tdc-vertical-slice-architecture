using BankingApp.Domain.Core;
using MediatR;

// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.ApplyProfitFee;

public record ApplyProfitFeeCommand(Guid HolderId, decimal ProfitFee) : IRequest, ICommand;