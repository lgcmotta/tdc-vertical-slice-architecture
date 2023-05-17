using AutoMapper;
using DesafioWarren.Application.Models;
using DesafioWarren.Domain.Aggregates;
using DesafioWarren.Domain.Entities;
using DesafioWarren.Domain.ValueObjects;
using FluentValidation.Results;

namespace DesafioWarren.Application.AutoMapper.Profiles
{
    public class AccountsProfile : Profile
    {
        public AccountsProfile()
        {

            CreateMap<Account, AccountModelBase>()
                .ConstructUsing(account => new AccountModelBase
                {
                    Name = account.Name,
                    Cpf = account.Cpf,
                    Email = account.Email,
                    PhoneNumber = account.PhoneNumber,
                    Currency = account.GetCurrencyIsoCode(),
                    Number = account.Number,
                })
                .ReverseMap()
                .ConstructUsing(accountModel => new Account(accountModel.Name
                    , accountModel.Email
                    , accountModel.PhoneNumber
                    , accountModel.Cpf
                    , Enumeration.GetItemByValue<Currency>(accountModel.Currency)));

            CreateMap<Account, AccountModel>()
                .ForMember(accountModel => accountModel.Balance
                    , options => options.MapFrom(account => account.GetBalance()))
                .ForMember(accountModel => accountModel.CurrencySymbol
                    , options => options.MapFrom(account => account.GetCurrencySymbol()))
                .ForMember(accountModel => accountModel.Currency
                    , options => options.MapFrom(account => account.GetCurrencyIsoCode()))
                .ReverseMap()
                .ConstructUsing(accountModel => new Account(accountModel.Name
                    , accountModel.Email
                    , accountModel.PhoneNumber
                    , accountModel.Cpf
                    , Enumeration.GetItemByValue<Currency>(accountModel.Currency)));
            
            CreateMap<AccountTransaction, AccountTransactionModel>()
                .ForMember(accountTransactionModel => accountTransactionModel.TransactionType, options => options.MapFrom(transaction => transaction.TransactionType.Value))
                .ForMember(accountTransactionModel => accountTransactionModel.BalanceAfterTransaction, options => options.MapFrom(transaction => transaction.BalanceBeforeTransaction + transaction.TransactionValue))
                .ReverseMap();

            CreateMap<ValidationFailure, Failure>().ReverseMap();
        }
    }
}