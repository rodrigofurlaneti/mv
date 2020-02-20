import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { OrderProvider } from '../../../providers/order/order';

@IonicPage()
@Component({
  selector: 'page-order-rate',
  templateUrl: 'order-rate.html',
})
export class OrderRatePage {
  pedido: any;

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private orderProvider: OrderProvider,
    public navParams: NavParams) {
    this.pedido = navParams.get("pedido");
  }

  ionViewDidLoad() {

  }

  notaPedido(nota: number) {
    if (!this.pedido.AvaliacaoPedido)
      this.pedido.AvaliacaoPedido = { NotaPedido: 0 };

    this.pedido.AvaliacaoPedido.NotaPedido = nota;
  }

  notaAplicativo(nota: number) {
    if (!this.pedido.AvaliacaoPedido)
      this.pedido.AvaliacaoPedido = { NotaAplicativo: 0 };

    this.pedido.AvaliacaoPedido.NotaAplicativo = nota;
  }

  avaliarPedido(pedido) {
    this.utils.showLoader('Registrando seu avaliação...');

    if (this.pedido.AvaliacaoPedido.ItensDeAcordoComAnuncio == "true")
      this.pedido.AvaliacaoPedido.ItensDeAcordoComAnuncio = true;
    else
      this.pedido.AvaliacaoPedido.ItensDeAcordoComAnuncio = false;

    this.orderProvider.saveRateOfOrder(pedido.Id, pedido.AvaliacaoPedido)
      .subscribe(data => {
        this.utils.hideLoader();
        this.utils.showToast("Avaliação registrada com Sucesso! Obrigado");
        this.orderProvider.get(this.pedido.Id)
          .subscribe(data => {
            this.navCtrl.setRoot("OrderDetailPage", { pedido: data });
          })
      }, error => {
        this.utils.hideLoader();
        this.utils.showToast(error.error);
      });
  }

}
