using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.RetrieveTokenHistory;

public record RetrieveTokenHistoryQuery(string Token) : IRequest<IEnumerable<RetrieveTokenHistoryResponse>>, IQuery;