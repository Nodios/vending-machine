import { useEffect } from 'react';
import { useQuery } from 'react-query';
import { ILoginResponse } from '../data/models/auth/ILoginResponse';
import localStorageProvider from '../data/storage/localStorageProvider';

export function useAuth() {
    const loginQuery = useQuery(['login'], () => {
        const data = localStorageProvider.get<ILoginResponse>('user');

        return data;
    });

    useEffect(() => {
        if (loginQuery.isSuccess) {
            const data = loginQuery.data;

            if (data != null && data.expiresAt <= new Date()) {
                // clear local storage
                localStorageProvider.clear();
                loginQuery.refetch();
            }
        }
    }, [loginQuery]);

    return loginQuery;
}
