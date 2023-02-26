import { useReducer } from 'react';
import { useInfiniteQuery } from 'react-query';
import { getSortString } from '../data/getSortString';
import { productsService } from '../data/services/productsService';

export function useProducts() {
    const [state, dispatch] = useReducer(filterReducer, {
        orderBy: '',
        orderAscending: false,
    });

    const changeOrderBy = (orderBy: string) => {
        if (orderBy === state.orderBy) {
            dispatch({ type: 'toggleOrderAscending', payload: null });
        } else {
            dispatch({ type: 'changeOrderBy', payload: orderBy });
            dispatch({ type: 'changeOrderAscending', payload: false });
        }
    };

    const {
        data,
        isLoading,
        isSuccess,
        isError,
        error,
        hasNextPage,
        fetchNextPage,
    } = useInfiniteQuery({
        queryKey: ['products', state.orderBy, state.orderAscending],
        queryFn: ({ pageParam = 0 }) =>
            productsService.find({
                skip: pageParam,
                take: 10,
                sort: getSortString(state.orderBy, state.orderAscending),
            }),
        getNextPageParam: (lastPage, pages) => {
            const totalItems = pages.reduce(
                (acc, p) => (acc += p.products.length),
                0,
            );

            return lastPage.totalItems > totalItems ? totalItems : null;
        },
        keepPreviousData: true,
        staleTime: 30 * 1000,
    });

    return {
        data,
        hasNextPage,
        fetchNextPage,
        isLoading,
        isSuccess,
        isError,
        error,
        changeOrderBy,
    };
}

type State = {
    orderBy: string;
    orderAscending: boolean;
};

type Action = {
    type: 'changeOrderBy' | 'changeOrderAscending' | 'toggleOrderAscending';
    payload: any;
};

function filterReducer(state: State, action: Action) {
    const { type, payload } = action;

    switch (type) {
        case 'changeOrderBy':
            return {
                ...state,
                orderBy: payload,
            };
        case 'changeOrderAscending':
            return {
                ...state,
                orderAscending: payload,
            };
        case 'toggleOrderAscending':
            return {
                ...state,
                orderAscending: !state.orderAscending,
            };
        default:
            return state;
    }
}
