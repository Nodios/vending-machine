import { useQuery, useQueryClient } from 'react-query';
import { Link } from 'react-router-dom';
import { ILoginResponse } from '../data/models/auth/ILoginResponse';
import localStorageProvider from '../data/storage/localStorageProvider';
import { useAuth } from '../hooks/useAuth';
import { useFunds } from '../hooks/useFunds';

export const AuthHeader = () => {
    const { data } = useAuth();

    return (
        <div>
            {data == null ? (
                <Link to="/login">Login</Link>
            ) : (
                <UserHeader userData={data} />
            )}
        </div>
    );
};

type UserHeaderProps = {
    userData: ILoginResponse;
};

const UserHeader: React.FC<UserHeaderProps> = (props) => {
    return (
        <div className="flex justify-center items-center">
            <div className="flex flex-col mr-3">
                <div>{props.userData.email}</div>
            </div>
            <AvailableFunds />
            <div>
                <Link
                    to="/deposit"
                    className="bg-green-500 text-white px-2 py-2 rounded-md"
                >
                    Add credits
                </Link>
            </div>
        </div>
    );
};

const AvailableFunds = () => {
    const { funds } = useFunds();
    return (
        <div className="flex flex-col items-center mr-3">
            <span className="text-xs">Credits</span>
            <span className="text-sm font-bold">{funds || 0}</span>
        </div>
    );
};
