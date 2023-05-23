using AutoMapper;
using BankingApp.Application.Models;
using BankingApp.Domain.Aggregates;
using BankingApp.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApp.Application.Commands.Handlers;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<Response> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<Account>(request.Account);

        account.DefineCurrency(request.Account.Currency);

        _accountRepository.Add(account);

        await _accountRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        var accountModel = _mapper.Map<AccountModel>(account);

        var response = new Response(accountModel);

        return response;
    }
}