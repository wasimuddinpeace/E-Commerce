import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { product } from "../shared/product";
import { Order, OrderItem } from '../shop/order';


@Injectable()
export class dataService{
    Products: product[] = [];


    //Initialize Order before  using it.
    public order: Order = new Order();

    constructor(private httpclient:HttpClient) {

    }

    private token: string = "";
    private tokenExpiration: Date;

    public get loginRequired(): boolean {

        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

    //login methods expect that creds are send and the login method return an observable 
    //the data service is then called by login component which will subscribe the data from data(boolean ) service and depending on the
    //result it will routet to the default page or the checkout page
    //
    login(creds): Observable<boolean> {
        return this.httpclient
            .post("/account/createtoken", creds)
            .pipe(
                map((data: any) => {
                this.token = data.token, //populated from the account controller
                        this.tokenExpiration = data.expiration // populated from the account controller
                return true;

            }));

    }

    public checkout(){
       //this is to check if the view model validation is failed due to noo order number, so we set it manually if
        //there exist no order number
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }
        return this.httpclient.post("/api/orders", this.order, {
            headers: new HttpHeaders().set( "Authorization","Bearer " + this.token)
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
    loadProducts(): Observable<boolean> {
        return this.httpclient.get("/api/products")
            .pipe(
                map((data: any[]) => {
                this.Products = data;
                return true;
            }));
    }

    public addToOrder(product: product) { 
        //check whether the orderItem exists or not
        //if (this.order) {
        //    this.order = new Order();
        //}
        let item: OrderItem = this.order.items.find(i => i.productId == product.id);
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
}