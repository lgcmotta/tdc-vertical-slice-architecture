﻿namespace BankingApp.Application.Models;

public class TransferModel : TransactionModel
{
    public string DestinationAccount { get; set; }
}