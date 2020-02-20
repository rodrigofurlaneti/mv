import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { OrderProvider } from '../../../providers/order/order';

/**
 * Generated class for the OrderListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-order-list',
  templateUrl: 'order-list.html',
})
export class OrderListPage {
  listaPedidos: any;
  isLoading: boolean = true;

  constructor(public navCtrl: NavController, 
    private utils: UtilsProvider,
    private orderProvider: OrderProvider,
    public navParams: NavParams) {
  }

  ionViewDidLoad() {
    this.utils.showLoader("carregando pagamentos...");
    this.orderProvider.list()
      .subscribe(
        data => {
          this.isLoading = false;
          this.listaPedidos = data;
          this.utils.hideLoader();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

  detalhes(pedido) {
    this.navCtrl.push("OrderDetailPage", {"pedido": pedido});
  }

}
