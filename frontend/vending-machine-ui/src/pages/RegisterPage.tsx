import { useSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useMutation } from 'react-query';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { Button } from '../components/Button';
import { usersService } from '../data/services/usersService';
import { Navigate } from 'react-router-dom';

const schema = yup.object({
    email: yup.string().email().required(),
    username: yup.string().required(),
    password: yup.string().min(8).required(),
    confirmPassword: yup
        .string()
        .oneOf([yup.ref('password'), ''], "Passwords don't match")
        .required(),
});
type Form = yup.InferType<typeof schema>;

export const RegisterPage = () => {
    const { enqueueSnackbar } = useSnackbar();
    const mutation = useMutation({
        mutationFn: usersService.register,
    });

    const {
        handleSubmit,
        register,
        formState: { errors },
    } = useForm<Form>({
        resolver: yupResolver(schema),
    });

    const onSubmit = (data: Form) => {
        mutation.mutate(data);
    };

    if (mutation.isSuccess) {
        return <Navigate to="/login" state={mutation.data} />;
    }

    return (
        <div className="w-1/2 m-auto mt-10">
            <form onSubmit={handleSubmit(onSubmit)}>
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
                    <label htmlFor="email" className="font-bold">
                        E-mail
                    </label>
                    <input
                        type="email"
                        {...register('email', { required: true })}
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
                <div className="flex flex-col mt-3 border-b">
                    <label htmlFor="confirmPassword" className="font-bold">
                        Confirm password
                    </label>
                    <input
                        type="password"
                        {...register('confirmPassword', { required: true })}
                        placeholder="********"
                    />
                    {errors.confirmPassword && (
                        <p className="text-red-500">
                            {errors.confirmPassword.message}
                        </p>
                    )}
                </div>

                <Button className="text-white bg-green-500 mt-10">
                    Register
                </Button>
            </form>
        </div>
    );
};
