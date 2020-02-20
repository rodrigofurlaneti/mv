import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ModalController } from 'ionic-angular';
import { HttpProvider } from '../../../providers/http/http';
import { PeopleProvider } from '../../../providers/people/people';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { PaymentProvider } from '../../../providers/payment/payment';

/**
 * Generated class for the OrderPaymentNewPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-order-payment-new',
  templateUrl: 'order-payment-new.html',
})
export class OrderPaymentNewPage {

  cartoes: any;
  agendamentos: any[] = [];
  pedido: any = {
    ListaCompra: null,
    Cartao: { Id: 0, Senha: ''},
    Endereco: {},
    Agendamento: {},
    Usuario: { Id: HttpProvider.userAuth.UsuarioId },
    Valor: '',
    Loja: { Id: 0 }
  };
  inicio: number = 1;
  quantidade: number = 5;
  
  constructor(public navCtrl: NavController,
    private peopleProvider: PeopleProvider,
    private paymentProvider: PaymentProvider,
    private utils: UtilsProvider,
    private mdlCtrl: ModalController,
    public navParams: NavParams) {
      this.pedido.Loja = this.navParams.get("loja");
  }

  ionViewDidLoad() {

    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para acidicionar o produto ao carrinho.');
      this.navCtrl.setRoot("LoginPage", { "redirect": "OrderPaymentNewPage" });
      return;
    }

    this.peopleProvider.getPessoa(HttpProvider.userAuth.PessoaId)
      .subscribe(
        data => {
          this.cartoes = data.Cartoes;
        }, error => {
          this.utils.showToast(error.error);
        }
      );
    this.loadCartoes();
  }

  loadCartoes(): any {
    this.paymentProvider.loadCartoes()
      .subscribe(
        data => {
          this.cartoes = data;
          if (data.length > 0)
            this.pedido.Cartao = data[0];
        }
      )
  }

  adicionarCartao() {
    let mdlCartoes = this.mdlCtrl.create("CartaoEditPage", { modalMode: true });
    mdlCartoes.onDidDismiss(
      d => this.reloadPessoa()
    )
    mdlCartoes.present();
  }

  reloadPessoa(): void {
    this.utils.showLoader("aguarde");
    this.peopleProvider.getPessoa(HttpProvider.userAuth.PessoaId)
      .subscribe(
        data => {
          this.utils.hideLoader();
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      );

    this.loadCartoes();
  }

  loadLista(): any {
    this.pedido.ListaCompra = ShoppingProvider.getCurrentLista();
    this.pedido.Usuario = { Id: HttpProvider.userAuth.UsuarioId }
    this.utils.hideLoader();
  }

  confirmarCompra() {
    this.navCtrl.push("OrderPaymentNewStep2Page", { "pedido": this.pedido });
  }

  selecionaCartao(cartao){
    this.pedido.Cartao = cartao;
  }

}
