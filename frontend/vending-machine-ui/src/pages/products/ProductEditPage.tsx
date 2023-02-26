import { useSnackbar } from 'notistack';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { useParams } from 'react-router-dom';
import { Button } from '../../components/Button';
import { getErrorMessages } from '../../data/getErrorMessages';
import { productsService } from '../../data/services/productsService';

type Props = {
    isAdd?: boolean;
};

type Form = {
    amountAvailable: number;
    cost: number;
    name: string;
};

export const ProductEditPage: React.FC<Props> = (props) => {
    const { id: productId } = useParams<{ id: string }>();
    const isAdd = props.isAdd;

    const { enqueueSnackbar } = useSnackbar();
    const queryClient = useQueryClient();
    const { register, handleSubmit, formState, reset } = useForm<Form>();

    const { data: product, isLoading: productLoading } = useQuery({
        queryKey: ['product', productId],
        queryFn: () => productsService.get(productId!),
        enabled: productId != null,
        staleTime: 60 * 60 * 1000,
    });

    useEffect(() => {
        if (product != null) {
            reset({
                amountAvailable: product.amountAvailable,
                cost: product.cost,
                name: product.name,
            });
        }
    }, [product, reset]);

    const mutation = useMutation({
        mutationFn: (variables: Form) => {
            if (isAdd) {
                return productsService.create(variables);
            }

            return productsService.update(productId!, variables);
        },
        onSuccess: (_, vars) => {
            queryClient.invalidateQueries(['products']);
            enqueueSnackbar(
                `${isAdd ? 'Added' : 'Updated'} product ${vars.name}.`,
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

    const onSubmit = (data: Form) => {
        mutation.mutate(data);
    };

    if (productLoading) {
        return (
            <div className="w-1/2 m-auto">
                Loading product data... Please hold.
            </div>
        );
    }

    return (
        <div className="w-1/2 m-auto">
            <h1>{isAdd ? 'Add new product' : `Edit ${product?.name}`}</h1>
            <form
                className="flex flex-col w-1/2"
                onSubmit={handleSubmit(onSubmit)}
            >
                <div className="flex flex-col mt-3 border-b">
                    <label htmlFor="name" className="font-bold">
                        Name
                    </label>
                    <input {...register('name', { required: true })} />
                </div>

                <div className="flex flex-col mt-3 border-b">
                    <label htmlFor="amountAvailable" className="font-bold">
                        Amount available
                    </label>
                    <input
                        type="number"
                        {...register('amountAvailable', {
                            required: true,
                            min: 0,
                        })}
                    />
                </div>

                <div className="flex flex-col mt-3 border-b">
                    <label htmlFor="cost" className="font-bold">
                        Cost
                    </label>
                    <input
                        type="number"
                        {...register('cost', {
                            required: true,
                            min: 1,
                        })}
                    />
                </div>

                <Button
                    disabled={!formState.isDirty || formState.isSubmitting}
                    className="mt-2"
                >
                    {isAdd ? 'Create' : 'Save'}
                </Button>
            </form>
        </div>
    );
};
