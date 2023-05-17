import React, { useState } from 'react';
import TabPanel from '../tab-panel';
import Transfer from '../transfer';
import Payment from '../payment';
import Withdraw from '../withdraw';
import TransactionsTable from '../transactions-table';
import { AppBar, Tabs, Tab,  makeStyles, Theme } from '@material-ui/core';
import { TransactionsPanelDiv } from './transactions-panel.styles';
import { Deposit } from '../deposit/deposit';

const useStyles = makeStyles((theme: Theme) => ({
    root: {
      flexGrow: 1,
      backgroundColor: theme.palette.background.paper,
      marginTop: '10px'
    },
    appBar: {
        display:'flex',
        flexDirection:'row',
        alignItems:'center',
        justifyContent:'center',
    }
  }));

export const TransactionsPanel:React.FC  = () => {
    const classes = useStyles();
    const [value, setValue] = useState(0);

    const tabPanes: { index: number, component: JSX.Element }[] = [
        { index: 0, component: <Deposit/> },
        { index: 1, component: <Transfer/> },
        { index: 2, component: <Payment/> },
        { index: 3, component: <Withdraw/> },
        { index: 4, component: <TransactionsTable/> },
    ]

    const tabs: { label: string }[] = [
        { label: 'Deposit' },
        { label: 'Transfer' },
        { label: 'Payment' },
        { label: 'Withdraw' },
        { label: 'My Transactions' }
    ]

    return (
        <TransactionsPanelDiv className={classes.root}>
                <AppBar className={classes.appBar} position='static'>
                    <Tabs value={value} onChange={(event, value) => setValue(value)}>
                        {tabs.map(({label}) => <Tab label={label}/>)}
                    </Tabs>
                </AppBar>
                {tabPanes.map(({index, component}) => <TabPanel value={value} index={index} children={component}/>)}
        </TransactionsPanelDiv>
    );
}

