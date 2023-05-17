import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import LogoutButton from '../logout';
import TextComponent from 'components/text-component';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    toolBar: {
        justifyContent: 'space-between'
    }
}));

export const ApplicationBar: React.FC = () => {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            <AppBar position='static'>
                <Toolbar className={classes.toolBar}>
                    <TextComponent variant='h6' text='Account Manager' />
                    <LogoutButton />
                </Toolbar>
            </AppBar>
        </div>
    );
}
