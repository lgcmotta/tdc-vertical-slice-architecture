import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { useAccount, useMsal } from '@azure/msal-react';
import { api } from 'api/api';
import { azureConfiguration } from 'api/authentication';

export const Callback: React.FC = () => {
    const { instance, accounts } = useMsal();

    const account = useAccount(accounts[0] || {});

    const history = useHistory();

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

            history.push('/home')
        })
    }, [account, history, instance])

    return <></>
}
