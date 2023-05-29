using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain;
using BankingApp.Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Accounts.API.Features.ChangeToken;

public class ChangeTokenCommandHandler : IRequestHandler<ChangeTokenCommand, ChangeTokenResponse>
{
    private readonly AccountHoldersDbContext _context;
    private readonly IAccountTokenGenerator _tokenGenerator;

    public ChangeTokenCommandHandler(AccountHoldersDbContext context, IAccountTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ChangeTokenResponse> Handle(ChangeTokenCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(account => account.Tokens.Where(token => token.Enabled))
            .FirstOrDefaultAsync(account => account.Tokens.Any(token => token.Value == request.Token), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account not found for token {request.Token}");
        }

        var newToken = _tokenGenerator.Generate();

        account.ChangeToken(newToken, DateTime.UtcNow);

        account.AddAccountTokenChangedDomainEvent();

        return new ChangeTokenResponse(newToken);
    }
}