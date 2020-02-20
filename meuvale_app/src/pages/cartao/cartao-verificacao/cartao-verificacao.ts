import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ViewController } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';

@IonicPage()
@Component({
  selector: 'page-cartao-verificacao',
  templateUrl: 'cartao-verificacao.html',
})
export class CartaoVerificacaoPage {

  cartao: any;
  private cardForm: FormGroup = new FormGroup({
    numero: new FormControl(''),
    validade: new FormControl(''),
    cvv: new FormControl(''),
    senha: new FormControl('', [Validators.required])
  });
  modalMode: any;

  constructor(public navCtrl: NavController, 
    private utils: UtilsProvider,
    private viewCtrl: ViewController,
    public navParams: NavParams) {
      this.modalMode = this.navParams.get("modalMode");
  }

  ionViewDidLoad() {
    this.cartao = this.navParams.get("cartao");
    if (this.cartao) {
      this.cardForm.setValue(
        {
          numero: this.cartao.Numero,
          validade: this.cartao.Validade,
          cvv: this.cartao.Cvv,
          senha: this.cartao.Senha
        }
      )
    }
  }

  dismiss(cartao) {
    this.viewCtrl.dismiss(cartao);
  }

  doSubmit(): void {
    if (this.cardForm.valid) {
      this.utils.showLoader("Aguarde...");
      this.utils.hideLoader();
      
      this.cartao.Senha = this.cardForm.get("senha").value;
      this.dismiss(this.cartao);

      this.navCtrl.pop();
      this.cardForm.reset();
    } else {
      this.utils.showToast("Preencha os campos corretamente!");
    }
  }
}
