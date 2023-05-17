import { Configuration} from '@azure/msal-browser';
import { azureConfiguration } from './azure-configuration';

export const msalConfiguration: Configuration = {
    auth: {
        authority: azureConfiguration.getMSALAuthority(),
        clientId: azureConfiguration.getMSALClientId(),
        postLogoutRedirectUri: azureConfiguration.getMSALPostLogoutRedirectUri(),
        redirectUri: azureConfiguration.getMSALRedirectUri(),
        navigateToLoginRequestUrl: azureConfiguration.getMSALNavigateToLoginRequestUrl(),
    },
    cache: {
        cacheLocation: azureConfiguration.getMSALCacheLocation(),
        storeAuthStateInCookie: azureConfiguration.getMSALStoreAuthStateInCookie(),
    },
};
