using MediatR;

namespace BankingApp.Transactions.API.Features.PeriodStatement;

public record PeriodStatementQuery(string Token, DateOnly? Start, DateOnly? End) : IRequest<IEnumerable<PeriodStatementModel>>;