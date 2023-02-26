import { useEffect, useState } from 'react';
import { useMutation, useQueryClient } from 'react-query';
import { Link } from 'react-router-dom';
import { IProduct } from '../data/models/products/IProduct';
import { paymentsService } from '../data/services/paymentsService';
import { useAuth } from '../hooks/useAuth';

type Props = {
    product: IProduct;
};

export const Product: React.FC<Props> = (props) => {
    const { data } = useAuth();

    return (
        <div className="py-6">
            <div className="flex max-w-md bg-white shadow-lg rounded-lg overflow-hidden h-full">
                <div
                    className="w-1/3 bg-cover"
                    style={{
                        backgroundImage: "url('https://picsum.photos/334/80')",
                    }}
                ></div>
                <div className="w-2/3 p-4 flex flex-col justify-between">
                    <div className="flex justify-between grow">
                        <h1 className="text-gray-900 font-bold text-2xl grow">
                            {props.product.name}
                        </h1>
                        {data?.id === props.product.sellerId && (
                            <Link to={`/product/${props.product.id}`}>
                                Edit
                            </Link>
                        )}
                    </div>
                    <p className="mt-2 text-gray-600 text-sm">
                        Available: {props.product.amountAvailable}
                    </p>
                    <p className="mt-2 text-gray-600 text-sm">
                        Product by: {props.product.sellerName}
                    </p>

                    <div className="flex item-center justify-between mt-3">
                        <h1 className="text-gray-700 font-bold text-xl">
                            {props.product.cost}
                        </h1>
                        <BuyButton product={props.product} />
                    </div>
                </div>
            </div>
        </div>
    );
};

const BuyButton: React.FC<{ product: IProduct }> = (props) => {
    const queryClient = useQueryClient();
    const [quantity, setQuantity] = useState(1);
    const buyMutation = useMutation({
        mutationFn: paymentsService.buy,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['products'] });
        },
    });

    useEffect(() => {
        if (buyMutation.isSuccess) {
            // TODO: show buy notification
        } else if (buyMutation.isError) {
            // TODO: show failed buy notification
        }
    });

    if (props.product.amountAvailable <= 0) {
        return (
            <div className="px-3 py-2 text-xs text-red-500 font-bold">
                Not available...
            </div>
        );
    }

    return (
        <div className="bg-green-800 text-white text-xs font-bold uppercase rounded flex items-center">
            <button
                type="button"
                className="px-3 h-full"
                onClick={() => setQuantity((c) => (c > 1 ? --c : c))}
            >
                -
            </button>
            <button
                type="button"
                onClick={() =>
                    buyMutation.mutate({
                        productId: props.product.id,
                        quantity: quantity,
                    })
                }
            >
                Buy {quantity}
            </button>
            <button
                type="button"
                className="px-3 h-full"
                onClick={() =>
                    setQuantity((c) =>
                        c < props.product.amountAvailable ? ++c : c,
                    )
                }
            >
                +
            </button>
        </div>
    );
};
