import { Outlet } from 'react-router-dom';

export const Admin = () => (
    // TODO: check if user is admin - if not prevent navigation to this page
    <div>
        <Outlet />
    </div>
);
