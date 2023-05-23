using System;

namespace BankingApp.Application.Models;

public class AccountModel : AccountModelBase, IEntityModel
{
    public Guid Id { get; set; }
        
    public string Balance { get; set; }

    public string CurrencySymbol { get; set; }

}