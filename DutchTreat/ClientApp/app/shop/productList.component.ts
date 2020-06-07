import { Component, OnInit } from '@angular/core';
import { dataService } from '../shared/dataService';
import { product } from '../shared/product';
import { Order } from './order';

@Component({
    selector: 'product-list',
    templateUrl: "./productList.component.html",
    styleUrls: ["./productList.component.css"]
})
export class ProductListComponent implements OnInit {

    constructor(private data: dataService) {
    }

    

    Products: product[] = [];

    ngOnInit(): void {
        this.data.loadProducts()
            .subscribe(success => {
                if (success) {
                    this.Products = this.data.Products;
                }
            });
    }

    addProduct(product: product) {
        this.data.addToOrder(product);
    }

    //BY using product component we are gpassing the product to the data service method addToorder and by using  addToOrder() method of data service it adds 
    //to order and the length of the order is achieved by cart component through data servoice i.e. this.data.order.length
}