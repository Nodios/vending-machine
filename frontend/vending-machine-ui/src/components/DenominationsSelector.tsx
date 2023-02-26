import { useReducer } from 'react';
import { Button } from './Button';

type Props = {
    denominations: number[];
    onApply: (coins: number[]) => void;
};

export const DenominationsSelector: React.FC<Props> = (props) => {
    const [state, dispatch] = useReducer(reducer, { coins: [], total: 0 });

    function onApply() {
        props.onApply(state.coins);
        dispatch({ type: 'reset', data: 0 });
    }

    return (
        <div className="grid grid-cols-3 gap-4">
            {props.denominations.map((d) => (
                <button
                    key={d}
                    type="button"
                    onClick={() => dispatch({ type: 'add', data: d })}
                >
                    <div className="p-6 rounded-lg bg-gray-200 text-black text-3xl overflow-hidden border border-black">
                        {d}
                    </div>
                </button>
            ))}
            <button
                type="button"
                onClick={() => dispatch({ type: 'reset', data: 0 })}
            >
                <div className="p-6 rounded-lg bg-gray-200 text-black text-3xl overflow-hidden border border-black">
                    Reset
                </div>
            </button>
            <div className="col-span-3">
                <div className="p-4 text-xl">Total: {state.total}</div>
            </div>
            <div className="col-span-3">
                <Button type="button" onClick={() => onApply()}>
                    Apply
                </Button>
            </div>
        </div>
    );
};

type Action = {
    type: 'add' | 'reset';
    data: number;
};
type State = {
    coins: number[];
    total: number;
};

function reducer(state: State, action: Action) {
    const { type, data } = action;

    switch (type) {
        case 'add':
            return {
                coins: [...state.coins, data],
                total: state.total + data,
            };
        case 'reset':
            return {
                coins: [],
                total: 0,
            };
        default:
            return state;
    }
}
