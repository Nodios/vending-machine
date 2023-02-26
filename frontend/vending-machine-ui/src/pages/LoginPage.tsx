import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
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

    const onSubmit = async (data: LoginForm) => {
        try {
            await login(data.username, data.password, data.rememberMe);
            navigate('/');
        } catch (ex) {
            console.error('LOGIN EX', ex);
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
