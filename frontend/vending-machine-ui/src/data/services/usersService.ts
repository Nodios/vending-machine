import { httpClient } from '../http/httpClient';
import { IRegisterResponse } from '../models/auth/IRegisterResponse';
import { IUserListResponse } from '../models/users/IUserListResponse';

type UsersFindRequest = {
    skip?: number;
    take?: number;
};

type UserRoleRequest = {
    userId: string;
    role: string;
};

type RegisterRequest = {
    email: string;
    username: string;
    password: string;
    confirmPassword: string;
};

class UsersService {
    async find(request: UsersFindRequest): Promise<IUserListResponse> {
        const response = await httpClient.get<IUserListResponse>('users', {
            params: request,
        });

        return response.data;
    }

    async addToRole(request: UserRoleRequest): Promise<any> {
        const response = await httpClient.put('users/role', request);

        return response;
    }

    async removeFromRole(request: UserRoleRequest): Promise<any> {
        const response = await httpClient.delete('users/role', {
            data: request,
        });

        return response;
    }

    async register(data: RegisterRequest): Promise<IRegisterResponse> {
        const response = await httpClient.post<IRegisterResponse>(
            'users',
            data,
        );

        return response.data;
    }
}

const usersService = new UsersService();

export { usersService };
