import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { HttpProvider } from '../../../providers/http/http';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { OrderProvider } from '../../../providers/order/order';

/**
 * Generated class for the OrderPaymentNewStep2Page page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-order-payment-new-step2',
  templateUrl: 'order-payment-new-step2.html',
})
export class OrderPaymentNewStep2Page {

  pedido: any;
  
  constructor(public navCtrl: NavController,
    private orderProvider: OrderProvider,
    private utils: UtilsProvider,
    public navParams: NavParams) {
      this.pedido = this.navParams.get("pedido");
      if (!HttpProvider.userAuth) {
        this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para acidicionar o produto ao carrinho.');
        this.navCtrl.setRoot("LoginPage", { "redirect": "OrderPaymentNewStep2Page" });
        return;
      }
  }

  confirmarCompra() {
    this.utils.showLoader("finalizando compra");
    this.orderProvider.finalize(this.pedido)
      .subscribe(
        data => {
          this.utils.hideLoader();
          this.utils.showAlert('Sucesso', data.Mensagem);
          this.navCtrl.setRoot("MainHomePage");
          this.cleanPedido();
          ShoppingProvider.setCurrentLista(null);
        }, error => {
          this.utils.showError(error.error);
        }
      )
  }

  private cleanPedido() {
    this.pedido = {
      ListaCompra: null,
      Cartao: {},
      Endereco: {},
      Agendamento: {},
      Usuario: { Id: HttpProvider.userAuth.UsuarioId }
    };
  }

  selecionaCartao(cartao){
    this.pedido.Cartao = cartao;
  }

}
