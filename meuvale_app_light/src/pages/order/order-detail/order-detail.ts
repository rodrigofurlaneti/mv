import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-order-detail',
  templateUrl: 'order-detail.html',
})
export class OrderDetailPage {
  pedido: any;
  lojista: boolean = false;

  constructor(public navCtrl: NavController, 
    public navParams: NavParams) {
    this.pedido = navParams.get("pedido");
    this.lojista = navParams.get("lojista");
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad OrderDetailPage');
  }

  avaliarPedido(pedido){
    let params = {
      pedido: pedido
    };
    this.navCtrl.push("OrderRatePage", params);
  }
}
