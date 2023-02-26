import { httpClient } from '../http/httpClient';
import { IProduct } from '../models/products/IProduct';
import { IProductsResponse } from '../models/products/IProductsResponse';
import { ProductsFilter } from '../models/products/ProductsFilter';

type ProductEditRequest = {
    amountAvailable: number;
    cost: number;
    name: string;
};

class ProductsService {
    async find(filter: ProductsFilter): Promise<IProductsResponse> {
        const response = await httpClient.get<IProductsResponse>('products', {
            params: filter,
        });

        return response.data;
    }

    async get(id: string): Promise<IProduct> {
        const response = await httpClient.get<IProduct>(`products/${id}`);

        return response.data;
    }

    create(data: ProductEditRequest) {
        return httpClient.post<IProductsResponse>('products', data);
    }

    update(id: string, data: ProductEditRequest) {
        return httpClient.put<IProductsResponse>(`products/${id}`, data);
    }

    delete(id: string) {
        return httpClient.delete<IProductsResponse>(`products/${id}`);
    }
}

const productsService = new ProductsService();

export { productsService };
