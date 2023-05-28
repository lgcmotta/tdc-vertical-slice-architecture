using BankingApp.Transactions.API.Infrastructure;
using BankingApp.Transactions.Domain.Exceptions;
using BankingApp.Transactions.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Transactions.API.Features.Transfers;

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
            .FirstOrDefaultAsync(account => account.Holder.Token == request.SenderToken, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        var receiver = await _context.Accounts
            .FirstOrDefaultAsync(account => account.Holder.Token == request.ReceiverToken, cancellationToken)
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

        var transaction = sender.Transfer(request.Amount, currency, receiver, DateTime.UtcNow);

        return new TransferTransactionResponse(transaction.Id, transaction.Type.Value);
    }
}