import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ModalController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { OrderProvider } from '../../../providers/order/order';
import { StoreProvider } from '../../../providers/store/store';
import { HttpProvider } from '../../../providers/http/http';
import { PageProvider } from '../../../providers/page/page';

/**
 * Generated class for the OrderStoreListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-order-store-list',
  templateUrl: 'order-store-list.html',
})
export class OrderStoreListPage {
  listaPedidos: any;
  isLoading: boolean = true;
  loja: any;
  lojista: number = 0;
  telaOrigem = "MainStorePage";
  tabOrigem = 1;

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private orderProvider: OrderProvider,
    private mdlCtrl: ModalController,
    public navParams: NavParams) {

    if (HttpProvider.userAuth.PerfilId == "99")
      this.telaOrigem = "MainAdminPage";

    PageProvider.setCurrentStore(this.telaOrigem, this.tabOrigem);

    StoreProvider.storeChange.subscribe(
      data => { this.loadData(); }
    );
  }

  ionViewWillEnter() {
    this.loadData();
  }

  loadData(): any {
    this.loja = StoreProvider.getCurrentStore();
    if (!this.loja)
      this.escolherLojas(true);
    else
      this.carregarPedidos();
  }

  escolherLojas(force: boolean = false) {
    if (HttpProvider.userAuth.PerfilId == "99")
      this.telaOrigem = "MainAdminPage";

    let mdl = this.mdlCtrl.create("StoreSelectPage", { modal: true, noClose: force, "usuarioLojista": HttpProvider.userAuth.PessoaId, "telaOrigem": this.telaOrigem, "tabOrigem": this.tabOrigem });
    mdl.onDidDismiss(
      data => {
        if (data) {
          this.loadData();
        }
      }
    )
    mdl.present();
  }

  detalhes(pedido) {
    this.navCtrl.push("OrderDetailPage", { "pedido": pedido, "lojista": true });
  }

  carregarPedidos() {
    this.utils.showLoader("carregando pedidos...");
    this.orderProvider.listByStore(this.loja.Id, 0, 10)
      .subscribe(
        data => {
          this.isLoading = false;
          this.listaPedidos = data;
          console.log(data);
          this.utils.hideLoader();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

  confirmarPedido(idPedido) {
    this.utils.showLoader("Confirmando o Pedido...")
    this.orderProvider.setAcceptOrder(idPedido)
      .subscribe(
        data => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.carregarPedidos();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

  recusarPedido(idPedido) {
    this.utils.showLoader(":-( Recusando o Pedido...")
    this.orderProvider.setRejectOrder(idPedido)
      .subscribe(
        data => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.carregarPedidos();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

  enviarPedido(idPedido) {
    this.utils.showLoader("Confirmando o envio do Pedido...")
    this.orderProvider.setSentOrder(idPedido)
      .subscribe(
        data => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.carregarPedidos();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

  liberarRetiradaPedido(idPedido) {
    this.utils.showLoader("Confirmando a liberação do Pedido...")
    this.orderProvider.setAvailableOrder(idPedido)
      .subscribe(
        data => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.carregarPedidos();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

  ConfirmarEntregaPedido(pedido) {
    this.utils.showLoader("Confirmando a entrega do Pedido...")
    this.orderProvider.setCollected(pedido.Id, pedido.QrCode.CodigoConfirmacao)
      .subscribe(
        data => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.carregarPedidos();
        }, error => {
          this.isLoading = false;
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

}
