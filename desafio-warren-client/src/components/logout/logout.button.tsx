import React, { useCallback } from 'react';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import IconButton from '@material-ui/core/IconButton';
import { useMsal } from '@azure/msal-react';

export const LogoutButton: React.FC = () => {
    const { instance } = useMsal();
    
    const handleLogout = useCallback(() => {

    instance.logout();
    
    }, [instance]);

    return ( 
        <IconButton
            color='inherit'
            onClick={handleLogout}>
            <ExitToAppIcon />
        </IconButton> 
    );
    
}