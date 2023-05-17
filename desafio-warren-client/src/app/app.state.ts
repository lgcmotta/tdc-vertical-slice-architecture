import { IAccount } from 'models/account';
import { ITransaction } from 'models/transaction';

export interface IAppState{
    user: IAccount;
    transactions: ITransaction[];
}

export const initialState: IAppState = {
    user: {} as IAccount,
    transactions: []
}