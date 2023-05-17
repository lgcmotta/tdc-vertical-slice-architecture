import React, { useEffect, useState } from 'react';
import AccountPanel from '../../components/account-panel';
import ApplicationBar from 'components/app-bar';
import TransactionsPanel from 'components/transactions-panel';
import HubComponent from 'components/hub-component';
import { useAccount, useMsal } from '@azure/msal-react';
import { api } from 'api/api';
import { azureConfiguration } from 'api/authentication';
import { HomeDiv } from './home.styles';


export const Home: React.FC = () => {
    const [isAuthenticating, setIsAuthenticating] = useState(true);

    const { instance, accounts, inProgress } = useMsal();

    const account = useAccount(accounts[0] || {});

    useEffect(() => {
        if(!account) return;

        const scopes = [ azureConfiguration.getMSALScopes() ];

        instance.acquireTokenSilent({
            scopes,
            account,
        }).then(response => {
            const { accessToken } = response;

            api.defaults.headers.common.Authorization = `Bearer ${accessToken}`;

            api.defaults.headers.common.Accept = 'application/json';

            setIsAuthenticating(false);
        })
    })

    return !isAuthenticating || inProgress !== 'none' ? (
        <HomeDiv>
            <ApplicationBar />
            <AccountPanel/>
            <TransactionsPanel />
            <HubComponent />
        </HomeDiv>
    ) : (
        <div/>
    );
}

