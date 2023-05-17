import { Pages } from 'pages';
import Callback from 'pages/callback';
import React, { lazy } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

const LazyHome = lazy(() => import('../../pages/home'));

const PrivateRoutes: React.FC = () => {
    return <Switch>
        <Route exact path={Pages.Home} component={LazyHome}></Route>
        <Route exact path={Pages.Callback} component={Callback}></Route>
        <Redirect to={Pages.Home} />
    </Switch>
}

export default PrivateRoutes;