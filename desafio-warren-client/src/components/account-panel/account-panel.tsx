import React, { useEffect } from 'react';
import TextComponent from 'components/text-component';
import PaperComponent from 'components/paper-component';
import { getAsync } from 'api';
import { AccountNameDiv, AccountPanelDiv, BalanceDiv } from './account-panel.styles';
import { IAccount } from 'models/account';
import { useAppContext } from 'app/app.context';
import { AppActionType } from 'app/app.actions';
import { makeStyles, Theme } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
        marginTop: '10px',
        width: '50%',
        minHeight:'auto',
        height: 'auto'
    }
}));

export const AccountPanel: React.FC = () => {
    const classes = useStyles();

    const { state, dispatch } = useAppContext();
    
    useEffect(() => {
        async function getAccount(){
            const response = await getAsync<IAccount>('/api/v1/accounts/myself');
            dispatch({type: AppActionType.UserAcquired, payload: response.payload });
        };
        getAccount();
    },[dispatch])

   const { user } = state;

    if(!user) return <></>

    return (
        <AccountPanelDiv>
            <AccountNameDiv>
                <PaperComponent className={classes.root}>
                    <TextComponent variant='h4' text={`Hello, ${user.name}!`} />
                </PaperComponent>
            </AccountNameDiv>
            <BalanceDiv>
                <PaperComponent className={classes.root}>
                    <TextComponent variant='h6' text='Your current balance is:'/>
                    <TextComponent variant='h4' text={user.balance} />
                </PaperComponent>
            </BalanceDiv>
        </AccountPanelDiv>
    );
}