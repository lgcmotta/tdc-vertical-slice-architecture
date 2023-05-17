import React, { createContext, useContext, useReducer } from 'react';
import { AppActionTypes } from './app.actions';
import { reducer } from './app.reducer';
import { IAppState, initialState } from './app.state';

interface IContextProps{
    state: IAppState;
    dispatch: React.Dispatch<AppActionTypes>;
}

const AppContext = createContext<IContextProps>({} as IContextProps);

const useAppContext = () => useContext(AppContext);

function useSelector<T>(selector: (state:IAppState) => T) {
    const { state } = useAppContext();

    return selector(state);
}

const AppContextProvider: React.FC = ({ children }) => {

    const [state, dispatch] = useReducer(reducer, initialState);
    
    return (
        <AppContext.Provider value={{ state, dispatch }} children={children}/>
    );
}


export { AppContextProvider, useAppContext, useSelector }