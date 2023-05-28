using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.MonthlyStatement;

public record MonthlyStatementQuery(string Token, int Year, int Month) : IRequest<IEnumerable<MonthlyStatementModel>>, IQuery;