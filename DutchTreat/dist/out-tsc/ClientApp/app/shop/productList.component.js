import { __decorate } from "tslib";
import { Component } from '@angular/core';
let ProductListComponent = class ProductListComponent {
    constructor(data) {
        this.data = data;
        this.Products = [];
    }
    ngOnInit() {
        this.data.loadProducts()
            .subscribe(success => {
            if (success) {
                this.Products = this.data.Products;
            }
        });
    }
    addProduct(product) {
        this.data.addToOrder(product);
    }
};
ProductListComponent = __decorate([
    Component({
        selector: 'product-list',
        templateUrl: "./productList.component.html",
        styleUrls: ["./productList.component.css"]
    })
], ProductListComponent);
export { ProductListComponent };
//# sourceMappingURL=productList.component.js.map