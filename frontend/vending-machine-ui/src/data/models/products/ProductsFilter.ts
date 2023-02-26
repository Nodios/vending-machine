export class ProductsFilter {
    skip: number;
    take: number;
    sort: string;

    constructor(
        skip: number = 0,
        take: number = 1,
        orderBy: string = 'dateCreated',
        orderDirection: 'asc' | 'desc' = 'desc',
    ) {
        this.skip = skip;
        this.take = take;
        this.sort = `${orderBy}|${orderDirection}`;
    }
}
