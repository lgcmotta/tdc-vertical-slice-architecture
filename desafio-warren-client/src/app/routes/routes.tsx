import React, { Suspense } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import PublicRoutes from './routes.public';
import PrivateRoutes from './routes.private';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';

export const Routes: React.FC = () => {
    const {inProgress} = useMsal();

    return inProgress !== 'none' ? ( <div/> ) : (
        <Router>
            <Suspense fallback={<div />}>
                <AuthenticatedTemplate>
                    <PrivateRoutes />
                </AuthenticatedTemplate>
                <UnauthenticatedTemplate>
                    <PublicRoutes />;
                </UnauthenticatedTemplate>
            </Suspense>
        </Router>
    );
}