import { __decorate } from "tslib";
import { Component } from '@angular/core';
let CartComponent = class CartComponent {
    constructor(data, router) {
        this.data = data;
        this.router = router;
    }
    onCheckout() {
        if (this.data.loginRequired) {
            //Force Login
            this.router.navigate(["login"]);
        }
        else {
            //go to checkout
            this.router.navigate(["Checkout"]);
        }
    }
};
CartComponent = __decorate([
    Component({
        selector: "my-cart",
        templateUrl: "./cart.component.html",
        styleUrls: []
    })
], CartComponent);
export { CartComponent };
//# sourceMappingURL=cart.component.js.map