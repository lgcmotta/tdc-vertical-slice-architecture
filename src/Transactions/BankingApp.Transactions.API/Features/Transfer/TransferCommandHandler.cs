using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.Transfer;

public class TransferCommandHandler : IRequestHandler<TransferCommand, TransferTransactionResponse>
{
    private readonly AccountsDbContext _context;

    public TransferCommandHandler(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<TransferTransactionResponse> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var sender = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Token == request.SenderToken, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        var receiver = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Token == request.ReceiverToken, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (sender is null)
        {
            throw new AccountNotFoundException($"Sender account not found for token {request.SenderToken}");
        }

        if (receiver is null)
        {
            throw new AccountNotFoundException($"Receiver account not found for token {request.SenderToken}");
        }

        var currency = Currency.ParseByValue<Currency>(request.Currency);

        var transaction = sender.TransferOut(receiver.Id, request.Amount, currency, DateTime.UtcNow);
        receiver.TransferIn(sender.Id, request.Amount, currency, DateTime.UtcNow);

        return new TransferTransactionResponse(transaction.Id, transaction.Type.Value);
    }
}