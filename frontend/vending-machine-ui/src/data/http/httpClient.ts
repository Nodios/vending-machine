import axios from 'axios';
import * as qs from 'qs';
import { IToken } from '../models/auth/IToken';
import localStorageProvider from '../storage/localStorageProvider';

function createInstance() {
    const client = axios.create({
        baseURL: process.env['REACT_APP_API'] + '/api',
        headers: {
            'Content-Type': 'application/json',
        },
        paramsSerializer: {
            serialize: (params) =>
                qs.stringify(params, { arrayFormat: 'comma' }),
        },
    });

    client.interceptors.request.use((config) => {
        const token = localStorageProvider.get<IToken>('token');

        if (token != null) {
            const { token: accessToken, expires } = token;

            // TODO: check if token expired

            config.headers['Authorization'] = 'Bearer ' + accessToken;
        }

        return config;
    });
    client.interceptors.response.use(
        (response) => {
            return response;
        },
        (error) => {
            if (error.response.status === 401) {
                localStorageProvider.remove('token');
                localStorageProvider.remove('user');
            }
            return Promise.reject(error);
        },
    );

    return client;
}

const httpClient = createInstance();

export { httpClient };
