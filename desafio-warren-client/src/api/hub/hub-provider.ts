import * as signalR from '@microsoft/signalr';

export const createHubConnection = () =>  new signalR.HubConnectionBuilder()
    .withUrl(`${process.env.REACT_APP_API}/accounts/hub`)
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();
