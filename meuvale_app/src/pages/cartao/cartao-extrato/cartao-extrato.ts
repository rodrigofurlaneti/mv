import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ViewController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';

@IonicPage()
@Component({
  selector: 'page-cartao-extrato',
  templateUrl: 'cartao-extrato.html',
})
export class CartaoExtratoPage {

  private extratoCartao: any;
  itensExtrato: any;
  modalMode: any;

  constructor(public navCtrl: NavController, 
    private viewCtrl: ViewController,
    private utils: UtilsProvider,
    public navParams: NavParams) {
      this.modalMode = this.navParams.get("modalMode");
      this.extratoCartao = this.navParams.get("extratoCartao");

      if (this.extratoCartao.Itens !== undefined && this.extratoCartao.Itens !== null)
        this.itensExtrato = this.extratoCartao.Itens;
  }

  ionViewDidLoad() {
  }

  dismiss() {
    this.viewCtrl.dismiss();
  }

  filterByDays(days){
    this.utils.showLoader("Filtrando...");

    if (days == 0)
    {
      this.itensExtrato = this.extratoCartao.Itens;
      this.utils.hideLoader();
      return;
    }

    var result = new Date();
    result.setHours(0,0,0,0);
    result.setDate(result.getDate() - days);
    
    this.itensExtrato = this.extratoCartao.Itens.filter(x => new Date(x.Data) >= result);
    
    this.utils.hideLoader();
  }
}
