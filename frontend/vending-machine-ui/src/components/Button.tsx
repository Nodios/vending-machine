import clsx from 'clsx';
import { ButtonHTMLAttributes } from 'react';

type Props = ButtonHTMLAttributes<HTMLButtonElement>;

export const Button: React.FC<Props> = (props) => {
    return (
        <button
            {...props}
            className={clsx(
                'p-2 rounded-md bg-blue-500 text-white',
                props.className,
            )}
        />
    );
};
