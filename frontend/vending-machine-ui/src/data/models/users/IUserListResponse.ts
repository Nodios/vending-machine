import { IUser } from './IUser';

export interface IUserListResponse {
    totalItems: number;
    users: IUser[];
}
