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
  avaliacao: boolean = true;
  permiteAlterarPreco: boolean = true;

  constructor(public navCtrl: NavController,
    private orderProvider: OrderProvider,
    private utils: UtilsProvider,
    public navParams: NavParams) {
    this.pedido = this.navParams.get("pedido");
    this.avaliacao = this.navParams.get("avaliacao");
    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para acidicionar o produto ao carrinho.');
      this.navCtrl.setRoot("LoginPage", { "redirect": "OrderPaymentNewStep2Page" });
      return;
    }

    if (this.pedido.Valor || this.pedido.Valor > 0) {
      this.permiteAlterarPreco = false;
    }
  }

  confirmarCompra() {
    this.utils.showLoader("finalizando compra");
    this.orderProvider.finalize(this.pedido)
      .subscribe(
        data => {
          this.utils.hideLoader();
          if (data.TipoMensagem == 1) {
            this.utils.showAlert('Falha', data.Mensagem);
          }
          else {
            this.utils.showAlert('Sucesso', data.Mensagem);
            //Desativa por enquanto, Infox já possuí o serviço de envio de SMS por transação
            // this.smsProvider.sendSMS("CompraAprovada",
            //   "Compra aprovada no valor de R$ " + this.pedido.Valor,
            //   HttpProvider.userAuth.Celular);

            let pedido: any;
            this.orderProvider.get(data.ObjetoRetorno.Id).subscribe(data => {
              pedido = data;
              if (this.avaliacao)
                this.avaliarPedido(pedido);
              else
                this.navCtrl.popToRoot();

              this.cleanPedido();
              ShoppingProvider.setCurrentLista(null);
            });
          }
        }, error => {
          this.utils.showError("Erro ao processar o pagamento, contate o suporte! ");
        }
      )
  }

  avaliarPedido(pedido) {
    let params = {
      pedido: pedido
    };
    this.navCtrl.setRoot("OrderRatePage", params);
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

}
