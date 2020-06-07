import { Component } from "@angular/core";
import { dataService } from '../shared/dataService';
import { Router } from '@angular/router';


@Component({
    selector: "checkout",
    templateUrl: "checkout.component.html",
    styleUrls: ['checkout.component.css']
})
export class CheckoutComponent {
    errorMessage: string;

    constructor(public data: dataService, private router: Router) {
    }

    onCheckout() {
        // TODO
        this.data.checkout()
            .subscribe(success => {
                if (success) {
                    alert("Order created");
                    this.router.navigate(["/"]);
                }
            },
                err => this.errorMessage = "Failed to save the order"); 
    }
}