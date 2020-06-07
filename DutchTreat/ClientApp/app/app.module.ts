import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ProductListComponent } from './shop/productList.component';
import { dataService } from './shared/dataService';
import { HttpClientModule } from '@angular/common/http';
import { CartComponent } from './shop/cart.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms'
import { ShopComponent } from './shop/shop.component';
import { CheckoutComponent } from './shop/checkout.component';
import { login } from './shop/login.component';

let routes = [
    {path: "", component: ShopComponent },
    { path: "Checkout", component: CheckoutComponent },
    {path:"login", component: login}

]

@NgModule({
  declarations: [
        AppComponent,
        ProductListComponent,
        CartComponent,
        ShopComponent,
        CheckoutComponent,
        login

  ],
  imports: [
      BrowserModule,
      HttpClientModule,
      RouterModule.forRoot(routes, {
          useHash: true,
          enableTracing: false // for debugging routes
      }),
      FormsModule

  ],
    providers: [
        dataService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
