import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

export const Admin = () => {
    // TODO: check if user is admin - if not prevent navigation to this page
    const auth = useAuth();

    if (auth.data == null) {
        return <Navigate to="/" />;
    }
    if (auth.data != null && !auth.data.roles.includes('Admin')) {
        return <Navigate to="/" />;
    }

    return (
        <div>
            <Outlet />
        </div>
    );
};
