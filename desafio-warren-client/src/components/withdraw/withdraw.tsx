import React, { useState } from 'react';
import TextComponent from 'components/text-component';
import MoneyInput from 'components/money-input';
import Notification from 'components/notification';
import PaperComponent from 'components/paper-component';
import { ITransactionResponse } from 'models/transaction';
import { putAsync } from 'api';
import { Theme, makeStyles } from '@material-ui/core/styles';
import { Button } from '@material-ui/core';
import { WithdrawButtonDiv, WithdrawDiv, WithdrawMoneyInputDiv, WithdrawTyphographyDiv } from './withdraw.styles';
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

export const Withdraw: React.FC = () => {
    const [value, setValue] = useState('');
    
    const [show, setShow] = useState(false);
    
    const [message, setMessage] =useState<string>('');
    
    const [severity, setSeverity] = useState<'error' | 'success'>('error');
    
    const classes = useStyles();
    
    const { id, currencySymbol } = useSelector(state => state.user)

    const handleWithdraw = () => {
        if(!value){
            setMessage(`You cannot withdraw ${currencySymbol}0!`);
            setSeverity('error');
            setShow(true);
            return;
        }

        const numberValue = parseFloat(value);

        async function putWithdraw(){
            await putAsync<ITransactionResponse>(`/api/v1/accounts/${id.toString()}/withdraw`, { value: numberValue })
            .then(response => {
                if(!response.failures.length){
                    setMessage(`${currencySymbol}${value} withdrawn successfully!`);
                    setSeverity('success');
                    setShow(true);
                    setValue('');
                }
            })
            .catch(err => {
                const failuresString = [...err.response.data.failures.map(failure => failure.errorMessage)].join(',\n');
                setMessage(failuresString);
                setSeverity('error');
                setShow(true);
            })

            
        }

        putWithdraw();

    }

    return (
        <WithdrawDiv>
            <PaperComponent className={classes.root}>
                <WithdrawTyphographyDiv>
                    <TextComponent variant='h5' text='How much do you want to withdraw?'/>
                </WithdrawTyphographyDiv>
                <WithdrawMoneyInputDiv>
                    <MoneyInput onChange={setValue} value={value} />
                </WithdrawMoneyInputDiv>
                <WithdrawButtonDiv>
                    <Button variant='contained' color='primary' fullWidth onClick={handleWithdraw}>
                        <TextComponent variant={undefined} text='Withdraw'/>
                    </Button>
                </WithdrawButtonDiv>
            </PaperComponent>
            <Notification show={show} setShow={setShow} message={message} severity={severity}/>
        </WithdrawDiv>
    );
}