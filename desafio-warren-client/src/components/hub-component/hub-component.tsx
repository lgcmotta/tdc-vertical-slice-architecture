import React, { memo, useEffect } from 'react';
import createHubConnection from 'api/hub';
import { Guid } from 'guid-typescript';
import { useAppContext, useSelector } from 'app/app.context';
import { AppActionType } from 'app/app.actions';


const SignalRHubComponent: React.FC = () => {
    
    const { dispatch } = useAppContext();

    const { id}  = useSelector(state => state.user);
    
    useEffect(() => {
        if(!id) return;

        const hubConnection = createHubConnection();

        hubConnection.on('AccountBalanceChanged', (accountBalance) => {
            dispatch({type: AppActionType.AccountBalanceChanged, payload: accountBalance });
        })

        hubConnection.start().then(a => {
            sendAccountInfoToHub(hubConnection, id);
        });

        hubConnection.onreconnected(() => sendAccountInfoToHub(hubConnection, id));

        function sendAccountInfoToHub(hubConnection:signalR.HubConnection, id: Guid | string) {
            if (hubConnection.connectionId)
                hubConnection.send('AppendAccountToList', id.toString(), hubConnection.connectionId);
        } 
    }, [id, dispatch])

    return (
        <></>
    );
}
 
export const HubComponent = memo(SignalRHubComponent);