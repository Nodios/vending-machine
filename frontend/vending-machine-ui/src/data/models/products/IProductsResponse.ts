import { IProduct } from './IProduct';

export interface IProductsResponse {
    products: IProduct[];
    totalItems: number;
}
