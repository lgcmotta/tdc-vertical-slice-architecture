using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.RetrieveYearlyStatement;

public record RetrieveYearlyStatementQuery(string Token, int Year) : IRequest<IEnumerable<RetrieveYearlyStatementModel>>, IQuery;