using BankingApp.Api.Filters;
using BankingApp.Application.Commands;
using BankingApp.Application.Models;
using BankingApp.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingApp.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiExceptionFilter]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IAccountsQueryWrapper _accountsQueryWrapper;

    public AccountsController(IMediator mediator, IAccountsQueryWrapper accountsQueryWrapper)
    {
        _mediator = mediator;
        _accountsQueryWrapper = accountsQueryWrapper;
    }

    [HttpGet("{accountId}/contacts")]
    public async Task<IActionResult> GetAccountsToTransferAsync([FromRoute] Guid accountId)
    {
        var response = await _accountsQueryWrapper.GetContactsAsync(accountId);

        return ReturnOk(response);
    }

    [HttpGet("{accountId}/transactions")]
    public async Task<IActionResult> GetAccountTransactionsAsync([FromRoute] Guid accountId)
    {
        var response = await _accountsQueryWrapper.GetAccountTransactions(accountId);

        return ReturnOk(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAccountAsync([FromBody] CreateAccountCommand createAccountCommand)
    {
        var response = await _mediator.Send(createAccountCommand);

        return FormatActionResult(Created($"'{HttpContext.Request.Path}/{((AccountModel)response.Payload).Id}", response), response);
    }

    [HttpPut]
    public async Task<IActionResult> PutAccountAsync([FromBody] UpdateAccountCommand updateAccountCommand)
    {
        var response = await _mediator.Send(updateAccountCommand);

        return ReturnOk(response);
    }

    [HttpPost("{accountId}/deposit")]
    public async Task<IActionResult> PostDepositAsync([FromRoute] Guid accountId, [FromBody] TransactionModel transaction)
    {
        var response = await _mediator.Send(new AccountDepositCommand(accountId, transaction.Value));

        return ReturnAccepted(response);
    }

    [HttpPut("{accountId}/transfer")]
    public async Task<IActionResult> PutTransferAsync([FromRoute] Guid accountId, [FromBody] TransferModel transferModel)
    {
        var response = await _mediator.Send(new AccountTransferCommand(accountId, transferModel.Value, transferModel.DestinationAccount));

        return ReturnAccepted(response);
    }

    [HttpPut("{accountId}/payment")]
    public async Task<IActionResult> PutPaymentAsync([FromRoute] Guid accountId, [FromBody] PaymentModel transaction)
    {

        var response = await _mediator.Send(new AccountPaymentCommand(accountId, transaction.Value, transaction.InvoiceNumber));

        return ReturnAccepted(response);
    }

    [HttpPut("{accountId}/withdraw")]
    public async Task<IActionResult> PutWithdrawAsync([FromRoute] Guid accountId, [FromBody] TransactionModel transaction)
    {
        var response = await _mediator.Send(new AccountWithdrawCommand(accountId, transaction.Value));

        return ReturnAccepted(response);
    }

    private IActionResult ReturnOk(Response response)
    {
        return FormatActionResult(Ok(response), response);
    }

    private IActionResult ReturnAccepted(Response response)
    {
        return FormatActionResult(Accepted(response), response);
    }

    private IActionResult FormatActionResult(IActionResult actionResult, Response response)
    {
        return response.IsErrorResponse() ? BadRequest(response) : actionResult;
    }
}