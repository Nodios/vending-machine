type ErrorsType<T = unknown> = {
    [key in keyof T]?: string[];
} & { GeneralErrors: string[] };

export interface IErrorResponse<T = unknown> {
    statusCode: number;
    message: string;
    errors: ErrorsType<T>;
}
