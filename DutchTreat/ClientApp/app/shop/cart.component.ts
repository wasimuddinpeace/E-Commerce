import { Component } from '@angular/core';
import { dataService } from '../shared/dataService';
import { Router } from '@angular/router';

@Component({

    selector: "my-cart",
    templateUrl:"./cart.component.html",
    styleUrls:[]
}) 

export class CartComponent {

    constructor(public data: dataService,private router: Router) { }
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
}