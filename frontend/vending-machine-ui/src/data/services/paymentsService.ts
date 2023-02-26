import { httpClient } from '../http/httpClient';
import { IBuyResponse } from '../models/payments/IBuyResponse';
import { IDenominationsResponse } from '../models/payments/IDenominationsResponse';
import { IDepositResponse } from '../models/payments/IDepositResponse';

type DepositRequest = {
    coins: number[];
};

type BuyRequest = {
    productId: string;
    quantity: number;
};

class PaymentsService {
    async getDenominations(): Promise<IDenominationsResponse> {
        const response = await httpClient.get<IDenominationsResponse>(
            'payments/denominations',
        );

        return response.data;
    }

    async deposit(data: DepositRequest): Promise<IDepositResponse> {
        const response = await httpClient.post<IDepositResponse>(
            'payments/deposit',
            data,
        );

        return response.data;
    }

    async buy(data: BuyRequest): Promise<IBuyResponse> {
        const response = await httpClient.post<IBuyResponse>(
            'payments/buy',
            data,
        );

        return response.data;
    }
}

const paymentsService = new PaymentsService();

export { paymentsService };
