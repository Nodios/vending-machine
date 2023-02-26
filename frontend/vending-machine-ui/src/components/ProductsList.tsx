import { IProductsResponse } from '../data/models/products/IProductsResponse';
import { useProducts } from '../hooks/useProducts';
import { Button } from './Button';
import { Product } from './Product';

export const ProductsList = () => {
    const { data, isSuccess, isError, hasNextPage, fetchNextPage } =
        useProducts();

    return (
        <div>
            <h1>Products</h1>
            {isSuccess && (
                <>
                    <div className="grid grid-cols-5 gap-4">
                        {data!.pages.map((page, index) => (
                            <List key={index} data={page} />
                        ))}

                        {hasNextPage && (
                            <div className="col-span-5 m-auto">
                                <Button
                                    type="button"
                                    onClick={() => fetchNextPage()}
                                >
                                    Load more...
                                </Button>
                            </div>
                        )}
                    </div>
                </>
            )}
            {isError && <Error />}
        </div>
    );
};

const List: React.FC<{ data: IProductsResponse }> = (props) => {
    return (
        <>
            {props.data.products.map((product) => (
                <Product key={product.id} product={product} />
            ))}
        </>
    );
};

const Error = () => (
    <div>Failed to retrieve list of products. Try refreshing your browser.</div>
);
