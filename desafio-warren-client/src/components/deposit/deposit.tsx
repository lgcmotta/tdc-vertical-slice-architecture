import React, { useState } from 'react';
import TextComponent from 'components/text-component';
import MoneyInput from 'components/money-input';
import Notification from 'components/notification';
import PaperComponent from 'components/paper-component';
import { Theme, makeStyles } from '@material-ui/core/styles';
import { Button } from '@material-ui/core';
import { DepositButtonDiv, DepositDiv, DepositMoneyInputDiv, DepositTyphographyDiv } from './deposit.styles';
import { useSelector } from 'app/app.context';
import { ITransactionResponse } from 'models/transaction';
import { postAsync } from 'api';


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


export const Deposit: React.FC = () => {
    const [value, setValue] = useState('');
    
    const classes = useStyles();
    
    const { id, currencySymbol } = useSelector(state => state.user)
    
    const [show, setShow] = useState(false);
    
    const [message, setMessage] =useState<string>('');
    
    const [severity, setSeverity] = useState<'error' | 'success'>('error');
    
    const handleDeposit = () => {
        if(!value){
            setMessage(`You cannot deposit ${currencySymbol}0!`);
            setSeverity('error');
            setShow(true);
            return;
        }

        const numberValue = parseFloat(value);

        async function postDeposit(){
            await postAsync<ITransactionResponse>(`/api/v1/accounts/${id.toString()}/deposit`, { value: numberValue }).then(response => {
                if(!response.failures.length){
                    setMessage(`${currencySymbol}${value} deposited successfully!`);
                    setSeverity('success');
                    setShow(true);
                    setValue('');
                }
            }).catch(err => {
                const failuresString = [...err.response.data.failures.map(failure => failure.errorMessage)].join(',\n');
                setMessage(failuresString);
                setSeverity('error');
                setShow(true);
            });
            
        }

        postDeposit();

    }

    return (
        <DepositDiv>
            <PaperComponent className={classes.root}>
                    <DepositTyphographyDiv>
                        <TextComponent variant='h5' text='How much do you want to deposit?'/>
                    </DepositTyphographyDiv>
                    <DepositMoneyInputDiv>
                        <MoneyInput onChange={setValue} value={value} />
                    </DepositMoneyInputDiv>
                    <DepositButtonDiv>
                        <Button variant='contained' color='primary' fullWidth onClick={handleDeposit}>
                            <TextComponent variant={undefined} text='Deposit'/>
                        </Button>
                    </DepositButtonDiv>
                <Notification show={show} setShow={setShow} message={message} severity={severity}/>
            </PaperComponent>
        </DepositDiv>
    );
}
