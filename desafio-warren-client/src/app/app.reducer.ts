import { AppActionType, AppActionTypes } from './app.actions';
import { IAppState } from './app.state';


export const reducer = (state: IAppState, action: AppActionTypes): IAppState => {
    switch (action.type) {
        case AppActionType.UserAcquired: {
            return { ...state, user: action.payload }
        }
        case AppActionType.AccountBalanceChanged: {
            return {...state, user: {...state.user, balance: action.payload }}
        }
    }
}