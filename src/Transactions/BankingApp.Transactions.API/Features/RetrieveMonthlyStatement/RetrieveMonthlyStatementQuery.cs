using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.RetrieveMonthlyStatement;

public record RetrieveMonthlyStatementQuery(string Token, int Year, int Month) : IRequest<IEnumerable<RetrieveMonthlyStatementModel>>, IQuery;