import { useQuery } from 'react-query';
import { ILoginResponse } from '../data/models/auth/ILoginResponse';
import localStorageProvider from '../data/storage/localStorageProvider';

export function useAuth() {
    const loginQuery = useQuery(['login'], () => {
        const data = localStorageProvider.get<ILoginResponse>('user');

        return data;
    });

    return loginQuery;
}
