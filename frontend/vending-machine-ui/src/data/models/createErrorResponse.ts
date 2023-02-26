import { IErrorResponse } from './IErrorResponse';

export function createErrorResponse(error: any) {
    if (error.response != null && error.response.data != null) {
        return error.response.data as IErrorResponse;
    }

    return {
        statusCode: 500,
        message: 'A server error has occurred.',
        errors: {
            GeneralErrors: ['A server error has occurred.'],
        },
    } as IErrorResponse;
}
