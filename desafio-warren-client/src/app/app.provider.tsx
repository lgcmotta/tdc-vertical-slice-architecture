import React from 'react';
import { PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { AppContextProvider } from './app.context';
import { msalConfiguration } from '../api/authentication/msal-configuration';

const AppProviders: React.FC = ({ children }) => {

    const msalInstance = new PublicClientApplication(msalConfiguration);
    
    return (
        <MsalProvider instance={msalInstance}>
            <AppContextProvider>{children}</AppContextProvider>
        </MsalProvider>
        );
    
}

export { AppProviders as default }