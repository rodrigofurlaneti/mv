import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, AlertController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PaymentProvider } from '../../../providers/payment/payment';
import { APPKEYS_CONFIG } from '../../../app/config/api.config';

@IonicPage()
@Component({
  selector: 'page-cartao-list',
  templateUrl: 'cartao-list.html',
})
export class CartaoListPage {

  cartao: any;
  cartoes: any[];
  detalhesCartao: any;
  extratoCartao: any;
  exibirSaldo: boolean = false;

  constructor(public navCtrl: NavController,
    private payment: PaymentProvider,
    private utils: UtilsProvider,
    private alertCtrl: AlertController,
    public navParams: NavParams) {
  }

  ionViewDidLoad() {
    this.utils.showLoader("Carregando");
    this.loadCartoes();
  }

  ShowCartaoVerificacao(cartao, operacao) {
    this.alertCtrl.create({
      title: (operacao === 1 ? "Verificar Saldo" : operacao === 2 ? "Gerar Extrato" : "??????"),
      message: "Informe a senha do cartão para realizar a operação!",
      enableBackdropDismiss: false,
      cssClass: "alertCartao",
      inputs: [{
        name: "Senha",
        type: 'password',
        max: 10,
        placeholder: "Senha do Cartão"
      }],
      buttons: [{
        text: 'OK',
        handler: (form) => {
          if (form.Senha !== undefined && form.Senha !== null && form.Senha !== "") {
            cartao.Senha = form.Senha;
            this.cartao = cartao;
            if (operacao === 1)
              this.buscaSaldo(cartao);
            else if (operacao === 2)
              this.obtemExtrato(cartao);
          }
        }
      },
      {
        text: "Fechar",
        handler: () => { }
      }]
    }).present();
  }

  saldo(cartao) {
    this.exibirSaldo = false;

    if (cartao.Senha === undefined || cartao.Senha === null || cartao.Senha === "") {
      this.ShowCartaoVerificacao(cartao, 1);
    }
    else {
      this.buscaSaldo(cartao);
    }
  }

  buscaSaldo(cartao) {
    if (cartao.Senha !== undefined && cartao.Senha !== null && cartao.Senha !== "") {
      this.utils.showLoader("Verificando Saldo...");
      this.payment.saldo(cartao)
        .subscribe(
          data => {
            this.detalhesCartao = data;

            cartao.SaldoDisponivel = "";
            cartao.LimiteCredito = "";
            cartao.DiaVencimento = "";

            if (this.detalhesCartao !== null && this.detalhesCartao !== undefined
              && this.detalhesCartao.Return === 0) {
              cartao.SaldoDisponivel =this.detalhesCartao.SaldoDisponivel;
              cartao.LimiteCredito = this.detalhesCartao.LimiteCredito;
              cartao.DiaVencimento = this.detalhesCartao.DiaVencimento;

              this.exibirSaldo = true;
            }
            else {
              cartao.Senha = "";
              if (this.detalhesCartao === undefined || this.detalhesCartao === null)
                this.utils.showToast("Não houve retorno! Contate o suporte.");
              else
                this.utils.showToast(this.detalhesCartao.StrResp);

              this.exibirSaldo = false;
            }

            this.utils.hideLoader();
          }, error => {
            cartao.Senha = "";
            this.utils.hideLoader();
            this.utils.showToast(error);
          }
        )
    }
  }

  extrato(cartao) {
    if (cartao.Senha === null || cartao.Senha === "" || cartao.Senha === undefined) {
      this.ShowCartaoVerificacao(cartao, 2);
    }
    else {
      this.obtemExtrato(cartao);
    }
  }

  obtemExtrato(cartao) {
    if (cartao.Senha !== null && cartao.Senha !== "" && cartao.Senha !== undefined) {
      this.utils.showLoader("Verificando Extrato...");
      this.payment.extrato(cartao, APPKEYS_CONFIG.estabelecimentoId)
        .subscribe(
          data => {
            this.extratoCartao = data;

            cartao.SaldoDisponivel = "";
            cartao.LimiteCredito = "";
            cartao.DiaVencimento = "";

            if (this.extratoCartao !== null && this.extratoCartao !== undefined
              && this.extratoCartao.Return === 0) {
              cartao.SaldoDisponivel = this.extratoCartao.SaldoDisponivel;
              cartao.LimiteCredito = this.extratoCartao.LimiteCredito;
              cartao.DiaVencimento = this.extratoCartao.DiaVencimento;

              this.navCtrl.push("CartaoExtratoPage", { extratoCartao: this.extratoCartao })
            }
            else {
              cartao.Senha = "";
              if (this.detalhesCartao === undefined || this.detalhesCartao === null)
                this.utils.showToast("Não houve retorno! Contate o suporte.");
              else
                this.utils.showToast(this.detalhesCartao.StrResp);
            }

            this.utils.hideLoader();
          }, error => {
            cartao.Senha = "";
            this.utils.hideLoader();
            this.utils.showToast(error);
          }
        )
    }
  }
  private loadCartoes() {
    this.payment.loadCartoes()
      .subscribe(
        data => {
          this.cartoes = data;
          this.utils.hideLoader();
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error);
        }
      )
  }

  editar(cartao: any): void {
    this.navCtrl.push('CartaoEditPage', { cartao: cartao })
  }

  remover(cartao) {
    this.alertCtrl.create({
      title: "Atenção!",
      message: "Confirma a exclusão deste cartão?",
      buttons: [
        "não",
        {
          text: "sim",
          handler: () => this.removeCartao(cartao)
        }
      ]
    }).present();
  }

  removeCartao(cartao: any) {
    this.utils.showLoader("removendo");
    this.payment.remove(cartao)
      .subscribe(
        data => {
          this.loadCartoes();

        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
  }

}
