import { useSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useLocation, useNavigate } from 'react-router-dom';
import { Button } from '../components/Button';
import { getErrorMessages } from '../data/getErrorMessages';
import { useLogin } from '../hooks/useLogin';

type LoginForm = {
    username: string;
    password: string;
    rememberMe: boolean;
};

export const LoginPage = () => {
    const { login } = useLogin();
    const { state } = useLocation();
    const { handleSubmit, register } = useForm<LoginForm>({
        defaultValues: {
            username: state?.username,
        },
    });
    const navigate = useNavigate();
    const { enqueueSnackbar } = useSnackbar();

    const onSubmit = async (data: LoginForm) => {
        try {
            await login(data.username, data.password, data.rememberMe);
            navigate('/');
        } catch (ex: any) {
            const messages = getErrorMessages(ex);
            if (messages != null && messages.length > 0) {
                enqueueSnackbar(messages[0], {
                    variant: 'error',
                });
            }
        }
    };

    return (
        <div className="w-1/2 m-auto mt-10">
            {state != null && <h2>Hi {state.username}, please log in.</h2>}
            <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col">
                <div className="flex flex-col mt-3 border-b">
                    <label htmlFor="username" className="font-bold">
                        Username
                    </label>
                    <input
                        {...register('username', { required: true })}
                        placeholder="Username"
                    />
                </div>

                <div className="flex flex-col mt-3 border-b">
                    <label htmlFor="password" className="font-bold">
                        Password
                    </label>
                    <input
                        type="password"
                        {...register('password', { required: true })}
                        placeholder="********"
                    />
                </div>

                <div className="flex mt-3 justify-center">
                    <input
                        id="remember-me"
                        type="checkbox"
                        className="mr-3"
                        {...register('rememberMe')}
                    />
                    <label htmlFor="remember-me">Remember me?</label>
                </div>

                <Button className="text-white bg-green-500 mt-10">
                    Log in
                </Button>
            </form>
        </div>
    );
};
