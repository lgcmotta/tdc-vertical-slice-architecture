using BankingApp.Domain.Core;
using MediatR;

// ReSharper disable ClassNeverInstantiated.Global
namespace BankingApp.Transactions.API.Features.Earnings;

public record ApplyEarningsCommand(Guid HolderId, decimal Earnings) : IRequest, ICommand;