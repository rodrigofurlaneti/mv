import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { OrderProvider } from '../../../providers/order/order';

/**
 * Generated class for the CupomDetailPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-cupom-detail',
  templateUrl: 'cupom-detail.html',
})
export class CupomDetailPage {
  produtoMv: any;
  loja: any;
  voucherResgatado: boolean = false;
  pedido: any;

  constructor(private navCtrl: NavController,
    private navParams: NavParams,
    private utils: UtilsProvider,
    private orderProvider: OrderProvider) {
    this.produtoMv = navParams.get("produto");
    this.loja = this.produtoMv.Loja ? this.produtoMv.Loja : this.produtoMv.Fornecedor;
  }

  ionViewWillEnter() {
    this.utils.showLoader("Aguarde...");
    this.orderProvider.getPedidoVoucherPorProduto(this.produtoMv.Id)
      .subscribe(
        data => {
          this.utils.hideLoader();
          if (data == null) return;
          this.pedido = data;
          this.voucherResgatado = true;
          this.utils.showToast('Voucher já resgatado para a oferta!', data.Mensagem);
        }, error => {
          this.utils.showError("Erro ao buscar voucher, contate o suporte! ");
        }
      )
  }

  voucher() {
    if (this.voucherResgatado) {
      this.navCtrl.push("VoucherPage", { pedido: this.pedido });
    }
    else {
      this.utils.showLoader("Aguarde, gerando seu voucher...");
      this.orderProvider.finalizeVoucher({ Fornecedor: this.loja, ProdutoPreco: this.produtoMv })
        .subscribe(
          data => {
            this.utils.hideLoader();
            if (data.TipoMensagem == 1) {
              this.utils.showAlert('Erro', data.Mensagem);
              return;
            } else {
              this.navCtrl.push("VoucherPage", { pedido: data.ObjetoRetorno });
            }
          }, error => {
            this.utils.showError("Erro ao processar o pedido, contate o suporte! ");
          }
        )
    }
  }

}
