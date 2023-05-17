import { Variant } from '@material-ui/core/styles/createTypography';
import Typography from '@material-ui/core/Typography';
import React from 'react';

interface ITextComponentProps{
    variant?: 'inherit' | Variant | undefined
    text: string;
}

export const TextComponent: React.FC<ITextComponentProps> = props => {
    const { variant, text} = props;
    return (
        <Typography variant={variant} style={{ fontFamily: 'Roboto', fontWeight: 200 }}>
            {text}
        </Typography>
    );
}