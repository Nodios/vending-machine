import { Link, useNavigate } from 'react-router-dom';
import { ILoginResponse } from '../data/models/auth/ILoginResponse';
import { useAuth } from '../hooks/useAuth';
import { useFunds } from '../hooks/useFunds';
import { useLogin } from '../hooks/useLogin';
import { Button } from './Button';

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
    const { logout } = useLogin();
    const { data } = useAuth();
    const navigate = useNavigate();
    return (
        <div className="flex justify-center items-center">
            <div className="flex flex-col mr-3">
                <div>{props.userData.email}</div>
            </div>
            {data!.roles.includes('Admin') ? (
                <div>
                    <Link
                        to="/admin"
                        className="bg-blue-500 text-white p-2 rounded-md"
                    >
                        Go to admin
                    </Link>
                    <Button
                        className="ml-2 bg-transparent text-black"
                        onClick={() => {
                            logout();
                            navigate('/');
                        }}
                    >
                        Logout
                    </Button>
                </div>
            ) : (
                <>
                    <AvailableFunds />
                    <div>
                        <Link
                            to="/deposit"
                            className="bg-green-500 text-white p-2 rounded-md"
                        >
                            Add credits
                        </Link>
                        <Button
                            className="ml-2 bg-transparent text-black"
                            onClick={() => logout()}
                        >
                            Logout
                        </Button>
                    </div>
                </>
            )}
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
