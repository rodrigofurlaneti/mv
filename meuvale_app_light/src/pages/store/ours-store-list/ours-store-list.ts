import { Component } from '@angular/core';
import { IonicPage, NavController, Platform, AlertController } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { Diagnostic } from '@ionic-native/diagnostic/ngx';

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
    private diagnostic: Diagnostic,
    private platform: Platform,
    private alertCtrl: AlertController,
    private utils: UtilsProvider,
    private storeProvider: StoreProvider) {
  }

  ionViewWillEnter() {
    this.loadLojas();
  }

  loadLojas() {
    this.utils.showLoader("localizando lojas");
    this.getLojas(0, 0);
    this.utils.hideLoader();
  }

  tryPermissionAgain() {
    if (this.platform.is('ios'))
      this.diagnostic.switchToSettings();
    else
      this.diagnostic.switchToLocationSettings();

    this.navCtrl.pop();
  }

  getLojas(lat: number, lng: number) {
    this.storeProvider.lojasUsuario(HttpProvider.userAuth.PessoaId, lat, lng)
      .subscribe(
        data => {
          this.lojas = data;
          for (var i = 0; i < this.lojas.length; i++) {
            var obj = {
              nome: this.lojas[i].Descricao,
              lat: parseFloat(this.lojas[i].Endereco.Latitude),
              lng: parseFloat(this.lojas[i].Endereco.Longitude)
            };
          }
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
