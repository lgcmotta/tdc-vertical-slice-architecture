import { Guid } from 'guid-typescript';

interface ITransactionResponse {
    status: string;
    occurence: Date;
    message: string;
}

interface ITransaction {
    id: Guid | string;
    transactionType: string;
    occurrence: Date;
    transactionValue: number;
    balanceBeforeTransaction: number,
    balanceAfterTransaction: number,
    earningsTaxPerDay: number | null
}