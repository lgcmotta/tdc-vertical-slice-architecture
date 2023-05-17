import React from 'react';
import Paper from '@material-ui/core/Paper';

interface IPaperComponentProps{
    className?: string;
}

export const PaperComponent: React.FC<IPaperComponentProps> = props => {
    const { children, className } = props;
    
    const style: any = { 
        display:'flex', 
        flexDirection:'column', 
        alignItems: 'center',
        justifyContent: 'center', 
        padding: '1%'
    };
    
    return (
        <Paper className={className} elevation={3} style={style}>
            {children}
        </Paper>
    );
}