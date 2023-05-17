import React, { useState } from 'react';
import AvailableAccounts from '../available-accounts';
import MoneyInput from 'components/money-input';
import Notification from '../notification';
import TextComponent from 'components/text-component';
import PaperComponent from 'components/paper-component';
import { Theme, makeStyles } from '@material-ui/core/styles';
import { Button } from '@material-ui/core';
import { TransferButtonDiv, TransferDiv, TransferMoneyInputDiv, TransferTyphographyDiv } from './transfer.styles';
import { ITransactionResponse } from 'models/transaction';
import { putAsync } from 'api';
import { useSelector } from 'app/app.context';

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


export const Transfer: React.FC = () => {
    const [transferValue, setTransferValue] = useState('');
    
    const [accountNumber, setAccountNumber] = useState('');
    
    const [show, setShow] = useState(false);
    
    const [message, setMessage] =useState<string>('');
    
    const [severity, setSeverity] = useState<'error' | 'success'>('error');
    
    const classes = useStyles();
    
    const { id, currencySymbol } = useSelector(state => state.user)

    if(!id) return <></>

    const handleTransfer = () => {
        if(!transferValue){
            setMessage(`You cannot transfer ${currencySymbol}0!`);
            setSeverity('error');
            setShow(true);
            return;
        }

        if(!accountNumber){
            setMessage(`You must choose a destination account!`);
            setSeverity('error');
            setShow(true);
            return;
        }

        const numberValue = parseFloat(transferValue);

        async function putTransfer(){
            await putAsync<ITransactionResponse>(`/api/v1/accounts/${id.toString()}/transfer`, { value: numberValue, destinationAccount: accountNumber })
                .then(response => {
                    if(!response.failures.length){
                        setMessage(`${currencySymbol}${transferValue} transferred successfully!`);
                        setSeverity('success');
                        setShow(true);
                        setTransferValue('');
                        setAccountNumber('');
                    }
                }).catch(err => {
                    const failuresString = [...err.response.data.failures.map(failure => failure.errorMessage)].join(',\n');
                    setMessage(failuresString);
                    setSeverity('error');
                    setShow(true);
                })

            
        }

        putTransfer();

    }

    return (
        <TransferDiv>
            <PaperComponent className={classes.root}>
                <TransferTyphographyDiv>
                    <TextComponent variant='h5' text='To whom do you want to transfer?'/>
                </TransferTyphographyDiv>
                <TransferTyphographyDiv>
                    <AvailableAccounts accountNumber={accountNumber} onSelectedAccountChange={setAccountNumber}/>
                </TransferTyphographyDiv>
                <TransferTyphographyDiv>
                    <TextComponent variant='h5' text='How much do you want to transfer?'/>
                </TransferTyphographyDiv>
                <TransferMoneyInputDiv>
                    <MoneyInput onChange={setTransferValue} value={transferValue} />
                </TransferMoneyInputDiv>
                <TransferButtonDiv>
                    <Button variant='contained' color='primary' fullWidth onClick={handleTransfer}>
                        <TextComponent variant={undefined} text='Transfer'/>
                    </Button>
                </TransferButtonDiv>
            </PaperComponent>
            <Notification show={show} setShow={setShow} message={message} severity={severity}/>
        </TransferDiv>
    );
}