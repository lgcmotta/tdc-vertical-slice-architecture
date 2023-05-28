using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Transactions.API.Features.YearlyStatement;

public record YearlyStatementQuery(string Token, int Year) : IRequest<IEnumerable<YearlyStatementModel>>, IQuery;