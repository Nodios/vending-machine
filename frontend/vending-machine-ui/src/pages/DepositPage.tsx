import { useSnackbar } from 'notistack';
import { useCallback } from 'react';
import { useMutation, useQuery } from 'react-query';
import { DenominationsSelector } from '../components/DenominationsSelector';
import { getErrorMessages } from '../data/getErrorMessages';
import { paymentsService } from '../data/services/paymentsService';
import { useFunds } from '../hooks/useFunds';

export const DepositPage = () => {
    const { enqueueSnackbar } = useSnackbar();
    const { updateFunds } = useFunds();
    const { data, isLoading } = useQuery(
        ['denominations'],
        paymentsService.getDenominations,
        {
            staleTime: 60 * 60 * 1000,
        },
    );
    const depositMutation = useMutation({
        mutationFn: paymentsService.deposit,
        onSuccess: (data) => {
            updateFunds(data.availableFunds);
            enqueueSnackbar(`Added ${data.amountAdded} to your account.`, {
                variant: 'success',
            });
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

    const depositFunds = useCallback(
        (coins: number[]) => {
            depositMutation.mutate({
                coins: coins,
            });
        },
        [depositMutation],
    );

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="flex justify-center items-center">
            <div className="mt-2">
                <DenominationsSelector
                    denominations={data!.denominations}
                    onApply={depositFunds}
                />
            </div>
        </div>
    );
};
