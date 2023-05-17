import { IAccount } from 'models/account';

export enum AppActionType {
    UserAcquired = 'UserAcquired',
    AccountBalanceChanged = 'AccountBalanceChanged'
}

export interface ISetUserAccountInfo {
    type: AppActionType.UserAcquired;
    payload: IAccount;
}

export interface IAccountBalanceChanged{
    type: AppActionType.AccountBalanceChanged,
    payload: string;
}

export type AppActionTypes = ISetUserAccountInfo | IAccountBalanceChanged;