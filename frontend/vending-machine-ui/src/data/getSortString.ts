export function getSortString(orderBy: string, orderAscending: boolean) {
    if (orderBy == null || orderBy.trim() === '') {
        return '';
    }

    return `${orderBy}|${orderAscending ? 'asc' : 'desc'}`;
}
