import React, { useCallback } from 'react';
import { Button } from '@material-ui/core';
import { BackgroundDiv, LoginButtonDiv, LoginDiv,LoginTitleDiv } from './login.style';
import { useMsal } from '@azure/msal-react';
import { azureConfiguration } from 'api/authentication';
import TextComponent from 'components/text-component';

export const Login: React.FC = () => {
    const imageUrl = `${process.env.PUBLIC_URL}/bg.svg`;

    const { instance } = useMsal();

    const handleLogin = useCallback(() => {
        try{
            instance.loginRedirect({ scopes: [ azureConfiguration.getMSALScopes() ]});
        } catch(error){
            console.log(error);
        }

    }, [instance])

    return (
        <BackgroundDiv style={{ backgroundImage: `url(${imageUrl})` }}>
            <LoginDiv>
                <LoginTitleDiv>
                    <TextComponent variant='h1' text='Motta Bank'/>
                </LoginTitleDiv>
                <LoginButtonDiv>
                    <TextComponent variant='h3' text='Welcome!'/>
                    <Button variant='contained' color='primary' fullWidth onClick={handleLogin}>
                        <TextComponent variant={undefined} text='Login'/>
                    </Button>
                </LoginButtonDiv>
            </LoginDiv>
        </BackgroundDiv>
    );
}
