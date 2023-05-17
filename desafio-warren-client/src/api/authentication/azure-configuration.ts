class AzureConfiguration{
    getMSALAuthority():string {
        return process.env.REACT_APP_AZURE_AUTHORITY as string;
    }
    getMSALClientId():string{
        return process.env.REACT_APP_AZURE_CLIENT_ID as string;
    }
    getMSALScopes():string{
        return process.env.REACT_APP_AZURE_SCOPES as string;
    }
    getMSALPostLogoutRedirectUri():string{
        return window.location.origin;
    }
    getMSALRedirectUri():string{
        return `${window.location.origin}/callback`;
    }
    getMSALNavigateToLoginRequestUrl():boolean{
        return true;
    }
    getMSALCacheLocation():string{
        return process.env.REACT_APP_AZURE_CACHE_LOCATION as string;
    }
    getMSALStoreAuthStateInCookie():boolean{
        return true;
    }


}

export const azureConfiguration = new AzureConfiguration();