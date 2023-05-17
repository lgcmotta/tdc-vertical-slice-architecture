import { Snackbar } from '@material-ui/core';
import React from 'react';
import MuiAlert, { AlertProps } from '@material-ui/lab/Alert';

interface INotificationProps {
    show: boolean;
    setShow: React.Dispatch<boolean>;
    message: string;
    severity:  'error' | 'success';
}

function Alert(props: AlertProps) {
    return <MuiAlert elevation={6} variant='filled' {...props} />;
  }


export const Notification: React.FC<INotificationProps> = (props: INotificationProps) => {
    const { show, setShow, message, severity} = props;

    const handleClose = (event?: React.SyntheticEvent, reason?: string) => {
        if (reason === 'clickaway') {
          return;
        }
    
        setShow(false);
      };

    return (
        <Snackbar open={show} autoHideDuration={6000} onClose={handleClose} anchorOrigin={{ vertical:'top', horizontal:'right' }}>
            <Alert onClose={handleClose} severity={severity}>
              {message}
            </Alert>
        </Snackbar>
    );
}