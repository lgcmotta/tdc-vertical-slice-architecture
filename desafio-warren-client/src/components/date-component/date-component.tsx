import React from 'react';
import Moment from 'react-moment'

interface IDateComponentProps{
    date:Date;
    format: string;
}

export const DateComponent: React.FC<IDateComponentProps> = (props:IDateComponentProps) => {
    const {date, format} = props;
    
    return (
        <Moment format={format}>
            {date}
        </Moment>
    );
}