using BankingApp.Accounts.API.Infrastructure;
using BankingApp.Accounts.Domain;
using BankingApp.Accounts.Domain.ValueObjects;
using MediatR;

namespace BankingApp.Accounts.API.Features.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountCreatedResponse>
{
    private readonly AccountHoldersDbContext _context;
    private readonly IAccountTokenGenerator _tokenGenerator;

    public CreateAccountCommandHandler(AccountHoldersDbContext context, IAccountTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AccountCreatedResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var token = _tokenGenerator.Generate();

        var currency = Currency.ParseByValue<Currency>(request.Currency);

        var account = new AccountHolder(request.FirstName, request.LastName, request.Document, token, currency);

        account.AddAccountCreatedDomainEvent();

        await _context.Accounts.AddAsync(account, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return new AccountCreatedResponse(account.GetCurrentToken());
    }
}