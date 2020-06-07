import { Component } from '@angular/core';
import { dataService } from '../shared/dataService';
import { Router } from '@angular/router';

@Component({
      selector:"login",
        templateUrl: "login.component.html",
        styleUrls: []
})
export class login {
    errorMessage: string;
    constructor(private data: dataService, private router: Router) {

    }

    //An Object that represents data on our form
    //Two way binding from the component to the html
    // From the html to the component
    public creds = {
        username:"",
        password:""
    };

    onLogin() {
        this.data.login(this.creds)
            .subscribe(success => {
                if (this.data.order.items.length == 0) {
                    this.router.navigate(["/"]);
                }
                else {
                    this.router.navigate(["Checkout"]);
                }

            }, error => this.errorMessage = "Failed to Login")

    }
}

//Note: That the creds are coming from  login cshtml --> account controller --- .data service --> login component 
