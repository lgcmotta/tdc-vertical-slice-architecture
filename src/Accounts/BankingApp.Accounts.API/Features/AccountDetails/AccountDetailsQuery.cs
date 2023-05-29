using BankingApp.Domain.Core;
using MediatR;

namespace BankingApp.Accounts.API.Features.AccountDetails;

public record AccountDetailsQuery(string Token) : IRequest<AccountDetailsResponse>, IQuery;