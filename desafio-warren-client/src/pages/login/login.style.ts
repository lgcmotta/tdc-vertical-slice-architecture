import styled from 'styled-components';

export const BackgroundDiv = styled.div`
    height: 100%;
    background-size: 100vw auto;
    background-repeat: no-repeat;
    background-position: top right;
`;

export const LoginDiv = styled.div`
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
`;

export const LoginTitleDiv =styled.div`
    flex-direction: column;
    display: flex;
    width: 100%;
    align-items: flex-start;
    padding-left: 10%;
    padding-top: 85px;
    margin: 0;
    height: 100%;
    max-height: 20%;
    flex-grow: 1;
`

export const LoginButtonDiv = styled.div`
    margin-top: 10%;
    margin-left: 10%;

    @media(min-height: 500px){
        margin-top: 15%;
    }

    @media(min-height: 700px){
        margin-top: 25%;
    }

    @media(min-height: 900px){
        margin-top: 25%;
    }

    @media(min-height: 1000px){
        margin-top: 15%;
    }

    @media(min-height:1080px){
        margin-top: 15%;
    }
    
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    align-items: center;
    width: auto;
    height: auto;
    min-height: 120px;
`
