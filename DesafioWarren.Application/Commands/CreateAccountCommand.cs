using DesafioWarren.Application.Models;
using MediatR;

namespace DesafioWarren.Application.Commands
{
    public class CreateAccountCommand : IRequest<Response>
    {
        public AccountModelBase Account { get; }

        public CreateAccountCommand(AccountModelBase account)
        {
            Account = account;
        }
    }
}