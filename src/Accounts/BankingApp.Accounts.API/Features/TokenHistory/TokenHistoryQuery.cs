using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.TokenHistory;

public record TokenHistoryQuery(string Token) : IRequest<IEnumerable<TokenHistoryResponse>>, IQuery;