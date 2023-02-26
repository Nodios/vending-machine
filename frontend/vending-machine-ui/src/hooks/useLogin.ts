import { useMutation, useQueryClient } from 'react-query';
import { ILoginResponse } from '../data/models/auth/ILoginResponse';
import { IToken } from '../data/models/auth/IToken';
import { createErrorResponse } from '../data/models/createErrorResponse';
import { loginService } from '../data/services/loginService';
import localStorageProvider from '../data/storage/localStorageProvider';
import { useFunds } from './useFunds';

export function useLogin() {
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: loginService.login,
    });
    const { updateFunds } = useFunds();

    async function login(
        username: string,
        password: string,
        rememberMe: boolean = false,
    ) {
        try {
            const result = await mutation.mutateAsync({
                username,
                password,
                rememberMe,
            });

            // save to local storage
            localStorageProvider.set<IToken>('token', {
                token: result.token,
                expires: result.expiresAt,
            });
            localStorageProvider.set<ILoginResponse>('user', result);
            queryClient.setQueryData(['login'], result);

            updateFunds(result.availableFunds);

            return result;
        } catch (ex) {
            throw createErrorResponse(ex);
        }
    }

    async function logout() {
        localStorageProvider.remove('token');
        localStorageProvider.remove('user');

        queryClient.setQueryData(['login'], null);
    }

    return {
        login,
        logout,
    };
}
