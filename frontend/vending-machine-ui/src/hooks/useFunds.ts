import { useQuery } from 'react-query';
import localStorageProvider from '../data/storage/localStorageProvider';

export function useFunds() {
    const { data: funds, refetch } = useQuery(['funds'], () => {
        const funds = localStorageProvider.get<number>('funds');
        if (funds != null) {
            return funds;
        }

        return 0;
    });

    function updateFunds(value: number) {
        localStorageProvider.set('funds', value);
        refetch();
    }

    return {
        funds,
        updateFunds,
    };
}
