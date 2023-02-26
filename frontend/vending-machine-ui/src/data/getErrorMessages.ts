import { AxiosError } from 'axios';
import { IErrorResponse } from './models/IErrorResponse';

export function getErrorMessages(error: AxiosError<IErrorResponse>) {
    if (error != null && error.response != null) {
        if (error.response.status === 401) {
            return ['Please log in.'];
        }
        return Object.values(error.response.data.errors).flat();
    }

    return null;
}
