import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';

/**
 * Generated class for the OursStoreListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-ours-store-list',
  templateUrl: 'ours-store-list.html',
})
export class OursStoreListPage {

  lojas: any;
  perfilLojista = HttpProvider.userAuth.PerfilId == "2";

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private storeProvider: StoreProvider) {
  }

  ionViewWillEnter() {
    this.loadLojas();
  }

  loadLojas() {
    this.utils.showLoader("localizando lojas");
    this.getLojas(0, 0);
  }

  getLojas(lat: number, lng: number) {
    this.storeProvider.lojasUsuario(HttpProvider.userAuth.PessoaId, lat, lng)
      .subscribe(
        data => {
          this.lojas = data.reverse(x => x.Id);
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error);
        },
        () => this.utils.hideLoader()
      );
  }

  qrCode(loja) {
    StoreProvider.setCurrentStore(loja);
    this.navCtrl.push("StoreQrcodeViewPage", { "loja": loja });
  }

  entrar(loja) {
    StoreProvider.setCurrentStore(loja);
    this.navCtrl.push("ShopPage", { "loja": loja, "visao": "lojista", "lojista": true });
  }

  goToStoreRegister(loja) {
    this.navCtrl.push("StoreRegisterPage", { "loja": loja });
  }
}
