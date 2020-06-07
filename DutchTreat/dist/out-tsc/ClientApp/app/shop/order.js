import * as _ from "lodash";
export class Order {
    constructor() {
        this.orderDate = new Date();
        this.items = new Array();
    }
    //subtotal: number;
    //field read only property which is called by the template
    get subtotal() {
        return _.sum(_.map(this.items, i => i.unitPrice * i.quantity));
    }
    ;
}
export class OrderItem {
}
//# sourceMappingURL=order.js.map