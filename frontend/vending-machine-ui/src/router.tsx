import { createBrowserRouter } from 'react-router-dom';
import { Admin } from './layout/Admin';
import { Main } from './layout/Main';
import { UserArea } from './layout/UserArea';
import { AdminHomePage } from './pages/admin/AdminHomePage';
import { DepositPage } from './pages/DepositPage';
import { HomePage } from './pages/HomePage';
import { LoginPage } from './pages/LoginPage';
import { RegisterPage } from './pages/RegisterPage';

export const router = createBrowserRouter([
    {
        element: <Main />,
        children: [
            {
                path: '/',
                element: <HomePage />,
            },
            {
                path: '/login',
                element: <LoginPage />,
            },
            {
                path: '/register',
                element: <RegisterPage />,
            },
            {
                element: <UserArea />,
                children: [
                    {
                        path: '/deposit',
                        element: <DepositPage />,
                    },
                ],
            },
            {
                path: '/admin',
                element: <Admin />,
                children: [
                    {
                        index: true,
                        element: <AdminHomePage />,
                    },
                ],
            },
        ],
    },
]);
