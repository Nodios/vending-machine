export interface ILoginResponse {
    availableFunds: number;
    email: string;
    expiresAt: Date;
    id: string;
    roles: string[];
    token: string;
    username: string;
}
