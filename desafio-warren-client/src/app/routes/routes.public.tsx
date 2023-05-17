import React, { lazy } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';
import { Pages } from '../../pages';

const LazyLogin = lazy(() => import('../../pages/login'));

const PublicRoutes: React.FC = () => {
    return <Switch>
        <Route exact path={Pages.Login} component={LazyLogin} />
        <Redirect to={Pages.Login} />
    </Switch>
}

export default PublicRoutes;