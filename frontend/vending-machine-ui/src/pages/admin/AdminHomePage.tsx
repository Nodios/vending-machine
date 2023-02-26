import { useSnackbar } from 'notistack';
import { useState } from 'react';
import { useInfiniteQuery, useMutation, useQueryClient } from 'react-query';
import { AddUserToRole } from '../../components/AddUserToRole';
import { getErrorMessages } from '../../data/getErrorMessages';
import { IUser } from '../../data/models/users/IUser';
import { IUserListResponse } from '../../data/models/users/IUserListResponse';
import { usersService } from '../../data/services/usersService';
import { useAuth } from '../../hooks/useAuth';

export const AdminHomePage = () => {
    const [selectedUser, setSelectedUser] = useState<IUser | undefined>();
    const { data, isLoading, isError, isSuccess, hasNextPage, fetchNextPage } =
        useInfiniteQuery({
            queryKey: ['users'],
            queryFn: ({ pageParam = 0 }) =>
                usersService.find({
                    skip: pageParam,
                    take: 10,
                }),
            getNextPageParam: (lastPage, pages) => {
                const totalItems = pages.reduce(
                    (acc, p) => (acc += p.users.length),
                    0,
                );

                return lastPage.totalItems > totalItems ? totalItems : null;
            },
            staleTime: 60 * 60 * 1000,
        });

    if (isLoading) {
        return <div>Loading users...</div>;
    }

    if (isError) {
        return <div>Failed to fetch users.</div>;
    }

    return (
        <div className="w-1/2 m-auto mt-10">
            <table className="w-full">
                <thead>
                    <tr className="font-bold border-b-2">
                        <td></td>
                        <td>E-mail</td>
                        <td>Username</td>
                        <td>Roles</td>
                        <td></td>
                    </tr>
                </thead>
                <tbody>
                    {data!.pages.map((page, index) => (
                        <UserList
                            key={index}
                            data={page}
                            page={index}
                            onUserSelect={setSelectedUser}
                        />
                    ))}
                </tbody>
            </table>

            {selectedUser != null && (
                <AddUserToRole
                    user={selectedUser}
                    onClose={() => setSelectedUser(undefined)}
                />
            )}
        </div>
    );
};

type UserListProps = {
    data: IUserListResponse;
    page: number;
    onUserSelect: (user: IUser | undefined) => void;
};

const UserList: React.FC<UserListProps> = (props) => {
    const { data } = useAuth();
    return (
        <>
            {props.data.users.map((u, uIdx) => (
                <tr key={u.id} className="border-b">
                    <td className="py-2">{10 * props.page + uIdx + 1}</td>
                    <td>{u.email}</td>
                    <td>{u.username}</td>
                    <td>
                        <div className="grid grid-cols-3 gap-3">
                            {u.roles.map((r) => (
                                <RoleButton key={r} user={u} role={r} />
                            ))}
                        </div>
                    </td>
                    <td>
                        {u.id !== data?.id && (
                            <button
                                type="button"
                                onClick={() => props.onUserSelect(u)}
                            >
                                Add role
                            </button>
                        )}
                    </td>
                </tr>
            ))}
        </>
    );
};

type RoleButtonProps = {
    user: IUser;
    role: string;
};

const RoleButton: React.FC<RoleButtonProps> = (props) => {
    const { data } = useAuth();
    const { enqueueSnackbar } = useSnackbar();
    const queryClient = useQueryClient();
    const mutation = useMutation({
        mutationFn: usersService.removeFromRole,
        onSuccess: () => {
            queryClient.invalidateQueries(['users']);
            enqueueSnackbar(
                `Removed ${props.user.username} from role ${props.role}.`,
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

    return (
        <div className="px-2 flex justify-between items-center">
            {props.role}
            {props.user.id !== data?.id && (
                <button
                    type="button"
                    className="m-2 px-3 bg-red-100"
                    onClick={() =>
                        mutation.mutate({
                            role: props.role,
                            userId: props.user.id,
                        })
                    }
                >
                    x
                </button>
            )}
        </div>
    );
};
