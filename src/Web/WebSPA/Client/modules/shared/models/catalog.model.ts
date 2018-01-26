import {ICatalogItem} from './catalogItem.model';

export interface ICatalog {
    PageIndex: number;
    Data: ICatalogItem[];
    PageSize: number;
    Count: number;
}
