using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.RetrievePeriodStatement;

public record RetrievePeriodStatementQuery(string Token, DateOnly? Start, DateOnly? End) : IRequest<IEnumerable<RetrievePeriodStatementModel>>, IQuery;