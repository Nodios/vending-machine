import { httpClient } from '../http/httpClient';
import { ILoginResponse } from '../models/auth/ILoginResponse';

type LoginRequest = {
    username: string;
    password: string;
    rememberMe?: boolean;
};

class LoginService {
    async login(data: LoginRequest): Promise<ILoginResponse> {
        const response = await httpClient.post<ILoginResponse>('login', data);

        return response.data;
    }
}

const loginService = new LoginService();

export { loginService };
