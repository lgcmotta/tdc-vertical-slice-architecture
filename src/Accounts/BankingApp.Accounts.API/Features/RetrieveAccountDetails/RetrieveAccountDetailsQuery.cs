using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.RetrieveAccountDetails;

public record RetrieveAccountDetailsQuery(string Token) : IRequest<RetrieveAccountDetailsResponse>, IQuery;