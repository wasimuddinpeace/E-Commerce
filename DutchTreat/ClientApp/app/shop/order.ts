import * as _ from "lodash";
export class Order {
    orderId: number;
    orderDate: Date = new Date();
    orderNumber: string;
    items: Array<OrderItem> = new Array<OrderItem>();

    //subtotal: number;
    //field read only property which is called by the template
    get subtotal(): number {
        return _.sum(_.map(this.items, i => i.unitPrice * i.quantity));
    };

}

export class OrderItem {
    id: number;
    quantity: number;
    unitPrice: number;
    productId: number;
    productCategory: string;
    productArtist: string;
    productSize: string;
    productPrice: number;
    productTitle: string;
    productArtId: string;
}