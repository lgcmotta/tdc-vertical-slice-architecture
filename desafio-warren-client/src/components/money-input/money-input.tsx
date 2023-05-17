import React from 'react';
import NumberFormat from 'react-number-format';
import TextField from '@material-ui/core/TextField';
import { createStyles, Theme, makeStyles } from '@material-ui/core/styles';
import { useSelector } from 'app/app.context';


const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      '& > *': {
        margin: theme.spacing(1),
      },
    },
  }),
);

export interface INumberFormatCustomProps {
    inputRef: (instance: NumberFormat | null) => void;
    onChange: (event: { target: { name: string; value: string } }) => void;
    name: string;
}

function NumberFormatCustom(props: INumberFormatCustomProps) {
    const { inputRef, onChange, ...other } = props;
    const { currencySymbol } = useSelector(state => state.user);
    return (
      <NumberFormat
        {...other}
        getInputRef={inputRef}
        onValueChange={(values) => {
          onChange({
            target: {
              name: props.name,
              value: values.value,
            },
          });
        }}
        thousandSeparator
        isNumericString
        prefix={currencySymbol}
      />
    );
}

export interface IMoneyInputFormatCustomProps{
    onChange: (string) => void;
    value: string;
}

export const MoneyInput: React.FC<IMoneyInputFormatCustomProps> = (props:IMoneyInputFormatCustomProps) => {
    const classes = useStyles();
    
    const { onChange, value} = props;   

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        onChange(event.target.value);
    };
    
    return (
        <div className={classes.root}>
          <TextField
            variant='outlined'
            label='Amount'
            value={value}
            onChange={handleChange}
            fullWidth
            name='numberformat'
            id='formatted-numberformat-input'
            InputProps={{inputComponent: NumberFormatCustom as any }}
          />
        </div>
    );
}
