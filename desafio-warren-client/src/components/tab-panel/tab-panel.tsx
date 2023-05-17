import React from 'react';

interface ITabPanelProps {
    children?: React.ReactNode;
    index: any;
    value: any;
  }

export const TabPanel: React.FC<ITabPanelProps> = (props:ITabPanelProps) => {
    const { children, value, index, ...other } = props;

    return (
      <div role='tabpanel' hidden={value !== index} id={`tabpanel-${index}`} aria-labelledby={`scrollable-prevent-tab-${index}`} {...other}>
        {value === index && children}
      </div>
    );
}