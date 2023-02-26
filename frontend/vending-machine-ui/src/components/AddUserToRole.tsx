import { useSnackbar } from 'notistack';
import { useForm } from 'react-hook-form';
import { useMutation, useQueryClient } from 'react-query';
import { getErrorMessages } from '../data/getErrorMessages';
import { IUser } from '../data/models/users/IUser';
import { usersService } from '../data/services/usersService';

type Props = {
    user: IUser;
    onClose: () => void;
};

// TODO: move this to server
const roles = ['Admin', 'Buyer', 'Seller'];

type Form = {
    role: string;
};

export const AddUserToRole: React.FC<Props> = (props) => {
    const { enqueueSnackbar } = useSnackbar();
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: usersService.addToRole,
        onSuccess: (_, vars) => {
            queryClient.invalidateQueries(['users']);
            enqueueSnackbar(
                `User ${props.user.username} added to role ${vars.role}`,
                {
                    variant: 'success',
                },
            );
        },
        onError: (error: any) => {
            const messages = getErrorMessages(error);
            if (messages != null && messages.length > 0) {
                enqueueSnackbar(messages[0], {
                    variant: 'error',
                });
            }
        },
    });
    const { register, handleSubmit } = useForm<Form>();

    const onSubmit = (data: Form) => {
        mutation.mutate({
            role: data.role,
            userId: props.user.id,
        });
    };

    return (
        <div className="mt-3 border p-2">
            <h1 className="text-xl font-bold border-b">
                Add {props.user.username} to role:
            </h1>
            <form onSubmit={handleSubmit(onSubmit)}>
                <select {...register('role')}>
                    {roles.map((r) => (
                        <option
                            key={r}
                            value={r}
                            disabled={props.user.roles.includes(r)}
                        >
                            {r}
                        </option>
                    ))}
                </select>
                <button>Add</button>
            </form>

            <div>
                <button type="button" onClick={() => props.onClose()}>
                    Close
                </button>
            </div>
        </div>
    );
};
