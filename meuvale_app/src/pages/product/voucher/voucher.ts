import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { OrderProvider } from '../../../providers/order/order';
import { UtilsProvider } from '../../../providers/utils/utils';

/**
 * Generated class for the VoucherPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-voucher',
  templateUrl: 'voucher.html',
})
export class VoucherPage {
  pedido: any;

  constructor(public navCtrl: NavController, 
    public navParams: NavParams,
    private orderProvider: OrderProvider,
    private utils: UtilsProvider) {
    this.pedido = navParams.get("pedido");
  }

  ionViewDidLoad() {
    this.utils.showLoader("Buscando seu voucher...");
    this.orderProvider.getPedidoVoucher(this.pedido.Id)
      .subscribe(
        data => {
          this.utils.hideLoader();
          if (data.TipoMensagem == 1) {
            this.utils.showAlert('Erro', data.Mensagem);
            return;
          } else {
            this.utils.showToast('Voucher resgatado com sucesso!', data.Mensagem);
          }
        }, error => {
          this.utils.showError("Erro ao processar o pagamento, contate o suporte! ");
        }
      )
  }

}
