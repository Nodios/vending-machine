import { useCallback, useEffect } from 'react';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { DenominationsSelector } from '../components/DenominationsSelector';
import { paymentsService } from '../data/services/paymentsService';

export const DepositPage = () => {
    const queryclient = useQueryClient();
    const { data, isLoading } = useQuery(
        ['denominations'],
        paymentsService.getDenominations,
        {
            staleTime: 60 * 60 * 1000,
        },
    );
    const depositMutation = useMutation({
        mutationFn: paymentsService.deposit,
    });

    const depositFunds = useCallback(
        (coins: number[]) => {
            depositMutation.mutate({
                coins: coins,
            });
        },
        [depositMutation],
    );

    useEffect(() => {
        if (depositMutation.isSuccess) {
            queryclient.setQueryData(
                'funds',
                depositMutation.data.availableFunds,
            );
        } else if (depositMutation.isError) {
            // TODO: show error notification
        }
    }, [depositMutation, queryclient]);

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
