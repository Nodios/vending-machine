import { useSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { getErrorMessages } from '../data/getErrorMessages';
import { useLogin } from '../hooks/useLogin';

type LoginForm = {
    username: string;
    password: string;
    rememberMe: boolean;
};

export const LoginPage = () => {
    const { login } = useLogin();
    const { handleSubmit, register } = useForm<LoginForm>();
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
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <input {...register('username')} placeholder="Username" />
                <input
                    type="password"
                    {...register('password')}
                    placeholder="********"
                />

                <input
                    id="remember-me"
                    type="checkbox"
                    {...register('rememberMe')}
                />
                <label htmlFor="remember-me">Remember me?</label>

                <button>Log in</button>
            </form>
        </div>
    );
};
