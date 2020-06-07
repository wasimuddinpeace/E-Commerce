import { __decorate } from "tslib";
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Order, OrderItem } from '../shop/order';
let dataService = class dataService {
    constructor(httpclient) {
        this.httpclient = httpclient;
        this.Products = [];
        //Initialize Order before  using it.
        this.order = new Order();
        this.token = "";
    }
    get loginRequired() {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }
    //login methods expect that creds are send and the login method return an observable 
    //the data service is then called by login component which will subscribe the data from data(boolean ) service and depending on the
    //result it will routet to the default page or the checkout page
    //
    login(creds) {
        return this.httpclient
            .post("/account/createtoken", creds)
            .pipe(map((data) => {
            this.token = data.token, //populated from the account controller
                this.tokenExpiration = data.expiration; // populated from the account controller
            return true;
        }));
    }
    checkout() {
        //this is to check if the view model validation is failed due to noo order number, so we set it manually if
        //there exist no order number
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }
        return this.httpclient.post("/api/orders", this.order, {
            headers: new HttpHeaders().set("Authorization", "Bearer " + this.token)
        })
            .pipe(map(response => {
            this.order = new Order();
            return true;
        }));
    }
    // data is the dat from the api call httpclient.get("/api/client")
    //Pipe so that we have multiple interceptors 
    //map is to map the data from the api call to the product array declarred in here.
    //set of parenthesis is important for data 
    loadProducts() {
        return this.httpclient.get("/api/products")
            .pipe(map((data) => {
            this.Products = data;
            return true;
        }));
    }
    addToOrder(product) {
        //check whether the orderItem exists or not
        //if (this.order) {
        //    this.order = new Order();
        //}
        let item = this.order.items.find(i => i.productId == product.id);
        if (item) {
            item.quantity++;
        }
        else {
            item = new OrderItem();
            item.productId = product.id;
            item.productArtist = product.artist;
            item.productArtId = product.artId;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.productTitle = product.title;
            item.unitPrice = product.price;
            item.quantity = 1;
            this.order.items.push(item);
        }
    }
};
dataService = __decorate([
    Injectable()
], dataService);
export { dataService };
//# sourceMappingURL=dataService.js.map