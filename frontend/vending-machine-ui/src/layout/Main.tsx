import { Link, Outlet } from 'react-router-dom';
import { AuthHeader } from '../components/AuthHeader';
import { useAuth } from '../hooks/useAuth';

import styles from './Main.module.css';

export const Main = () => {
    const auth = useAuth();
    return (
        <div>
            <div className={styles.nav}>
                <div className="flex justify-between items-center px-4 py-3 border-b">
                    <Link to="/">
                        <h1 className="text-2xl">Vending machine</h1>
                    </Link>
                    <AuthHeader />
                </div>
            </div>

            <main className={styles.main}>
                <Outlet />
            </main>
        </div>
    );
};
