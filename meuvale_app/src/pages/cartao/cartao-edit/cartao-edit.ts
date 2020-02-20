import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ViewController } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PaymentProvider } from '../../../providers/payment/payment';
import { Observable } from 'rxjs';
import { HttpProvider } from '../../../providers/http/http';

@IonicPage()
@Component({
  selector: 'page-cartao-edit',
  templateUrl: 'cartao-edit.html',
})
export class CartaoEditPage {

  private cartao: any;
  private cardForm: FormGroup = new FormGroup({
    numero: new FormControl('', [Validators.required]),
    validade: new FormControl('', [Validators.required]),
    cvv: new FormControl('', [Validators.required])
  });
  modalMode: any;

  constructor(public navCtrl: NavController, 
    private utils: UtilsProvider,
    private paymentProvider: PaymentProvider,
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
          cvv: this.cartao.Cvv
        }
      )
    }
  }

  dismiss() {
    this.viewCtrl.dismiss();
  }

  doSubmit(): void {
    if (this.cardForm.valid) {
      this.utils.showLoader("Aguarde...");
      const _cartao = this.getCard();
      let service: Observable<any>;

      if (this.cartao) {
        service = this.paymentProvider.update(_cartao);
      } else {
        service = this.paymentProvider.save(_cartao);
      }

      service.subscribe(
        () => {//sucesso
          this.utils.hideLoader();
          if(!this.cartao) {
            this.utils.showAlert("Sucesso", "Cartãos salvo com sucesso");
            this.navCtrl.pop();
            this.cardForm.reset();
          } else {
            this.navCtrl.pop();
            this.utils.showAlert("Sucesso", "Cartão alterado com sucesso");
          }
        }, error => {
          this.utils.hideLoader();          
          this.utils.showToast(error.error);
        }
      )

    } else {
      this.utils.showToast("Preencha os campos corretamente!");
    }
  }

  private getCard(): any{
    const formData = this.cardForm.value;
    return {
      Numero: formData.numero,
      Cvv: formData.cvv,
      Validade: formData.validade,
      Id: this.cartao ? this.cartao.Id: null,
      Pessoa: {
        Id: HttpProvider.userAuth.PessoaId
      }
    }
  }

}
