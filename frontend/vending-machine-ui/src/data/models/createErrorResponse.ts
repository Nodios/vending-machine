import { IErrorResponse } from './IErrorResponse';

export function createErrorResponse(error: any) {
    if (error.response != null) {
        if (error.response.data != null && error.response.data !== '') {
            return error.response.data as IErrorResponse;
        }
        if (error.response.status === 401) {
            return {
                statusCode: 401,
                message: 'Unauthorized',
                errors: {
                    GeneralErrors: ['Please log in.'],
                },
            } as IErrorResponse;
        }
    }

    return {
        statusCode: 500,
        message: 'A server error has occurred.',
        errors: {
            GeneralErrors: ['A server error has occurred.'],
        },
    } as IErrorResponse;
}
