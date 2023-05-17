import { AuthenticationState } from 'react-aad-msal';

interface AuthReducerState {
    initializing: boolean;
    initialized: boolean;
    idToken: any;
    accessToken: string | null;
    state: AuthenticationState;
    account?: any;
}
