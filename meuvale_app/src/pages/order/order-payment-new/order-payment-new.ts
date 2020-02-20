import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController, NavParams, ModalController } from 'ionic-angular';
import { HttpProvider } from '../../../providers/http/http';
import { PeopleProvider } from '../../../providers/people/people';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { PaymentProvider } from '../../../providers/payment/payment';
import { Slides } from 'ionic-angular';

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
  
  @ViewChild(Slides) slides: Slides;

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
  cartao: any;
  avaliacao: boolean = true;
  
  constructor(public navCtrl: NavController,
    private peopleProvider: PeopleProvider,
    private paymentProvider: PaymentProvider,
    private utils: UtilsProvider,
    private mdlCtrl: ModalController,
    public navParams: NavParams) {
      this.pedido.Loja = this.navParams.get("loja");
      this.pedido.Valor = this.navParams.get("valor");
      this.avaliacao = this.navParams.get("avaliacao");
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
          if(data && data.Cartoes)
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

  confirmarCompra() {
    this.pedido.Cartao = this.cartoes[this.slides.getActiveIndex()];
    this.navCtrl.push("OrderPaymentNewStep2Page", { "pedido": this.pedido, avaliacao: this.avaliacao });
  }
}
