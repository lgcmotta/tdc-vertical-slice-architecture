import React, { useState } from 'react';
import MoneyInput from 'components/money-input';
import Notification from 'components/notification';
import PaperComponent from 'components/paper-component';
import TextComponent from 'components/text-component';
import { Theme, makeStyles } from '@material-ui/core/styles';
import { Button, TextField } from '@material-ui/core';
import { PaymentButtonDiv, PaymentDiv, PaymentMoneyInputDiv, PaymentTyphographyDiv, PaymentInvoiceNumberDiv } from './payments.styles';
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

export const Payment: React.FC = () => {
    const [paymentValue, setPaymentValue] = useState('');
    
    const [invoiceNumber, setInvoiceNumber] = useState('');
    
    const [show, setShow] = useState(false);
    
    const [message, setMessage] =useState<string>('');
    
    const [severity, setSeverity] = useState<'error' | 'success'>('error');
    
    const classes = useStyles();
    
    const { id, currencySymbol } = useSelector(state => state.user)

    const handleTransfer = () => {
        if(!paymentValue){
            setMessage(`You cannot pay ${currencySymbol}0!`);
            setSeverity('error');
            setShow(true);
            return;
        }

        if(!invoiceNumber){
            setMessage('You must inform an invoice number or code!');
            setSeverity('error');
            setShow(true);
            return;
        }

        const numberValue = parseFloat(paymentValue);

        async function putPayment(){
            await putAsync<ITransactionResponse>(`/api/v1/accounts/${id.toString()}/payment`, { value: numberValue, invoiceNumber: invoiceNumber })
            .then(response => {
                if(!response.failures.length){
                    setMessage(`${currencySymbol}${paymentValue} paid successfully!`);
                    setSeverity('success');
                    setShow(true);
                    setPaymentValue('');
                    setInvoiceNumber('');
                }
            })
            .catch(err => {
                const failuresString = [...err.response.data.failures.map(failure => failure.errorMessage)].join(',\n');
                setMessage(failuresString);
                setSeverity('error');
                setShow(true);
            })
        }
        putPayment();
    }

    return (
        <PaymentDiv>
            <PaperComponent className={classes.root}>
                <PaymentTyphographyDiv>
                    <TextComponent variant='h5' text='Please enter the code or invoice number'/>
                </PaymentTyphographyDiv>
                <PaymentInvoiceNumberDiv>
                    <TextField variant='outlined' label='Invoice code or number' value={invoiceNumber} onChange={event => setInvoiceNumber(event.target.value)} fullWidth/>
                </PaymentInvoiceNumberDiv>
                <PaymentTyphographyDiv>
                    <TextComponent variant='h5' text="What's the total amount for this invoice?"/>
                </PaymentTyphographyDiv>
                <PaymentMoneyInputDiv>
                    <MoneyInput onChange={setPaymentValue} value={paymentValue} />
                </PaymentMoneyInputDiv>
                <PaymentButtonDiv>
                    <Button variant='contained' color='primary' fullWidth onClick={handleTransfer}>
                        <TextComponent variant={undefined} text='Pay'/>
                    </Button>
                </PaymentButtonDiv>
            </PaperComponent>
            <Notification show={show} setShow={setShow} message={message} severity={severity}/>
        </PaymentDiv>
    );
}