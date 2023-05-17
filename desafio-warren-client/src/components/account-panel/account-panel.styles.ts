import styled from 'styled-components';

export const AccountPanelDiv = styled.div`
    width: 100%;
    height: auto;
    max-height: 300px;
    display: flex;
    flex-direction: row;
    margin-top: 10px;
    @media(max-height: 500px){
        max-height: 200px;
    }
`;

export const AccountNameDiv = styled.div`
    width: 100%;
    display:flex;
    flex-direction:column;
    align-items:flex-start;
    justify-content: center;
    padding-left: 1%;
`

export const BalanceDiv = styled.div`
    width: 100%;
    display:flex;
    flex-direction:column;
    align-items:flex-end;
    justify-content: center;
    padding-right: 1%;
`;