import React, { useEffect, useState } from 'react';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';
import { IAccountBase } from 'models/account';
import { getAsync } from 'api';
import { useSelector } from 'app/app.context';


interface IAvailableAccounts{
    onSelectedAccountChange: (accountNumber: string) => void;
    accountNumber: string;
}

export const AvailableAccounts:React.FC<IAvailableAccounts> = (props:IAvailableAccounts) => {
  
    const { onSelectedAccountChange, accountNumber} = props;
    
    const { id } = useSelector(state => state.user);

    const [accounts, setAccounts] = useState<IAccountBase[]>([]);

    useEffect(() => {
        async function getAvailableAccounts():Promise<IAccountBase[]>{
            const response = await getAsync<IAccountBase[]>(`/api/v1/accounts/${id.toString()}/contacts`);
    
            return response.payload;
        }

        getAvailableAccounts().then(availableAccounts => setAccounts(availableAccounts));

    },[id])

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        onSelectedAccountChange((event.target as HTMLInputElement).value);
    };

    return (
        <FormControl component='fieldset'>
            <FormLabel component='legend'>Available Accounts</FormLabel>
            <RadioGroup aria-label='gender' name='gender1' value={accountNumber} onChange={handleChange}>
                {accounts.length && accounts.map(account => <FormControlLabel key={account.number} value={account.number} control={<Radio/>} label={account.name}/>)}
            </RadioGroup>
        </FormControl>
    );
}