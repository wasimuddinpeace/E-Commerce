import { __decorate } from "tslib";
import { Component } from '@angular/core';
let login = class login {
    constructor(data, router) {
        this.data = data;
        this.router = router;
        //An Object that represents data on our form
        //Two way binding from the component to the html
        // From the html to the component
        this.creds = {
            username: "",
            password: ""
        };
    }
    onLogin() {
        this.data.login(this.creds)
            .subscribe(success => {
            if (this.data.order.items.length == 0) {
                this.router.navigate(["/"]);
            }
            else {
                this.router.navigate(["Checkout"]);
            }
        }, error => this.errorMessage = "Failed to Login");
    }
};
login = __decorate([
    Component({
        selector: "login",
        templateUrl: "login.component.html",
        styleUrls: []
    })
], login);
export { login };
//Note: That the creds are coming from  login cshtml --> account controller --- .data service --> login component 
//# sourceMappingURL=login.component.js.map