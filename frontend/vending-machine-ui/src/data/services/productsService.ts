import { httpClient } from '../http/httpClient';
import { IProductsResponse } from '../models/products/IProductsResponse';
import { ProductsFilter } from '../models/products/ProductsFilter';

class ProductsService {
    async find(filter: ProductsFilter): Promise<IProductsResponse> {
        const response = await httpClient.get<IProductsResponse>('products', {
            params: filter,
        });

        return response.data;
    }
}

const productsService = new ProductsService();

export { productsService };
