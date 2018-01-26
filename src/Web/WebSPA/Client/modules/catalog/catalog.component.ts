import { Component, OnInit }    from '@angular/core';
import { Subscription }         from 'rxjs/Subscription';

import { CatalogService }       from './catalog.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { ICatalog }             from '../shared/models/catalog.model';
import { ICatalogItem }         from '../shared/models/catalogItem.model';
import { ICatalogType }         from '../shared/models/catalogType.model';
import { ICatalogBrand }        from '../shared/models/catalogBrand.model';
import { IPager }               from '../shared/models/pager.model';
import { BasketWrapperService}  from '../shared/services/basket.wrapper.service';
import { SecurityService }      from '../shared/services/security.service';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'esh-catalog .esh-catalog',
    styleUrls: ['./catalog.component.scss'],
    templateUrl: './catalog.component.html'
})
export class CatalogComponent implements OnInit {
    brands: ICatalogBrand[];
    types: ICatalogType[];
    catalog: ICatalog;
    brandSelected: number;
    typeSelected: number;
    paginationInfo: IPager;
    authenticated: boolean = false;
    authSubscription: Subscription;
    errorReceived: boolean;

    constructor(private service: CatalogService, private basketService: BasketWrapperService, private configurationService: ConfigurationService, private securityService: SecurityService) {
        this.authenticated = securityService.IsAuthorized;
    }

    ngOnInit() {

        // Configuration Settings:
        if (this.configurationService.isReady) 
            this.loadData();
        else
            this.configurationService.settingsLoaded$.subscribe(x => {
                this.loadData();
            });

        // Subscribe to login and logout observable
        this.authSubscription = this.securityService.authenticationChallenge$.subscribe(res => {
            this.authenticated = res;
        });
    }

    loadData() {
        this.getBrands();
        this.getCatalog(10, 0);
        this.getTypes();
    }

    onFilterApplied(event: any) {
        event.preventDefault();
        this.getCatalog(this.paginationInfo.itemsPage, this.paginationInfo.actualPage, this.brandSelected, this.typeSelected);
    }

    onBrandFilterChanged(event: any, value: number) {
        event.preventDefault();
        this.brandSelected = value;
    }

    onTypeFilterChanged(event: any, value: number) {
        event.preventDefault();
        this.typeSelected = value;
    }

    onPageChanged(value: any) {
        console.log('catalog pager event fired' + value);
        event.preventDefault();
        this.paginationInfo.actualPage = value;
        this.getCatalog(this.paginationInfo.itemsPage, value);
    }

    addToCart(item: ICatalogItem) {
        this.basketService.addItemToBasket(item);
    }

    getCatalog(pageSize: number, pageIndex: number, brand?: number, type?: number) {
        this.errorReceived = false;
        this.service.getCatalog(pageIndex, pageSize, brand, type)
            .catch((err) => this.handleError(err))
            .subscribe(catalog => {
                this.catalog = catalog;
                this.paginationInfo = {
                    actualPage : catalog.PageIndex,
                    itemsPage : catalog.PageSize,
                    totalItems : catalog.Count,
                    totalPages: Math.ceil(catalog.Count / catalog.PageSize),
                    items: catalog.pageSize
                };
        });
    }

    getTypes() {
        this.service.getTypes().subscribe(types => {
            this.types = types;
            let alltypes = { Id: null, Name: 'All' };
            this.types.unshift(alltypes);
        });
    }

    getBrands() {
        this.service.getBrands().subscribe(brands => {
            this.brands = brands;
            let allBrands = { Id: null, Name: 'All' };
            this.brands.unshift(allBrands);
        });
    }

    private handleError(error: any) {
        this.errorReceived = true;
        return Observable.throw(error);
    }
}
