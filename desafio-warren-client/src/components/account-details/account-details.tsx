import TextComponent from 'components/text-component';
import React from 'react';
import { AccountDetailsDiv } from './account-details.styles';

interface IAccountDetailsProps{
    cpf: string;
    number: string;
    name: string;
    currency: string;
    email: string
}

export const AccountDetails: React.FC<IAccountDetailsProps> = (props: IAccountDetailsProps) => {
    const { currency, cpf, number, name, email } = props;

    return (
        <AccountDetailsDiv>
            <TextComponent variant='overline'text={`Name: ${name}`}/>
            <TextComponent variant='overline'text={`Email: ${email}`}/>
            <TextComponent variant='overline'text={`CPF: ${cpf}`}/>
            <TextComponent variant='overline'text={`Account Number: ${number}`}/>
            <TextComponent variant='overline'text={`Currency: ${currency}`}/>
        </AccountDetailsDiv>
    );
}