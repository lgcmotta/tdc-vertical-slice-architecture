import { Guid } from 'guid-typescript';

interface IAccountBase {
    name: string;
    email: string;
    cpf: string;
    phoneNumber: string;
    number: string;
    currency: string;
}

interface IAccount extends IAccountBase {
    id: Guid | string;
    balance: string;
    currencySymbol: string;
}
