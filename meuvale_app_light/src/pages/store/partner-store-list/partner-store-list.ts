import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { PartnerProvider } from '../../../providers/partner/partner';
import { UtilsProvider } from '../../../providers/utils/utils';

/**
 * Generated class for the PartnerStoreListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-partner-store-list',
  templateUrl: 'partner-store-list.html',
})
export class PartnerStoreListPage {

  lojas: any;

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private partnerProvider: PartnerProvider) {
  }

  ionViewDidLoad() {
    this.utils.showLoader("localizando parceiros");
    this.getLojas(0, 0);
          this.utils.hideLoader();
  }

  getLojas(lat: number, lng: number) {
    this.partnerProvider.fornecedores()
      .subscribe(
        data => {
          this.lojas = data;
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error);
        },
        () => this.utils.hideLoader()
      );
  }

  entrar(loja) {
    PartnerProvider.setCurrentPartner(loja);
    this.navCtrl.push("ShopPage", { "loja": loja, "visao": "lojista" });
  }

  goToStoreRegister(loja) {
    this.navCtrl.push("StoreRegisterPage", { "loja": loja, "fornecedor": true });
  }
}
