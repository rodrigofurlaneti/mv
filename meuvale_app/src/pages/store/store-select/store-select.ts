import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ViewController, Platform, AlertController } from 'ionic-angular';
import { Geolocation } from '@ionic-native/geolocation';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoreProvider } from '../../../providers/store/store';
import { MainStorePage } from '../../main-store/main-store';
import { Diagnostic } from '@ionic-native/diagnostic/ngx';
import { GeolocationProvider } from '../../../providers/google/geolocation';
import { MainAdminPage } from '../../main-admin/main-admin';

/**
 * Generated class for the StoreSelectPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-store-select',
  templateUrl: 'store-select.html',
})
export class StoreSelectPage {

  lojas: any;
  usuarioLojista: number = 0;
  telaOrigem: string;
  tabOrigem: number = 1;
  isModal: boolean;
  noClose: boolean;
  fechado: boolean = false;


  constructor(public navCtrl: NavController,
    private geo: Geolocation,
    private diagnostic: Diagnostic,
    private geoProvider: GeolocationProvider,
    private platform: Platform,
    private alertCtrl: AlertController,
    private utils: UtilsProvider,
    private viewCtrl: ViewController,
    private storeProvider: StoreProvider,
    public navParams: NavParams) {

    if (navParams.get("usuarioLojista"))
      this.usuarioLojista = this.navParams.get("usuarioLojista");

    if (navParams.get("telaOrigem"))
      this.telaOrigem = this.navParams.get("telaOrigem");

    if (navParams.get("tabOrigem"))
      this.tabOrigem = this.navParams.get("tabOrigem");

    this.isModal = this.navParams.get("modal");

    this.noClose = this.navParams.get("noclose");

  }

  ionViewDidLoad() {
    this.loadLojas();
  }

  loadLojas() {
    this.utils.showLoader("localizando lojas");

    if (!this.usuarioLojista) {
      this.geo.getCurrentPosition({
        timeout: 30000,
        enableHighAccuracy: true
      }).then(
        resp => {
          if (resp && resp.coords)
            this.getLojas(resp.coords.latitude, resp.coords.longitude);
          else {
            this.utils.hideLoader();
            this.utils.showToast("erro ao consultar o gps");
          }
        }
      );
    }
    else
      this.getLojasPorPessoaId(0, 0);
  }

  tryPermissionAgain() {
    if (this.platform.is('ios'))
      this.diagnostic.switchToSettings();
    else
      this.diagnostic.switchToLocationSettings();

    this.navCtrl.pop();
  }

  showRequestAlert(code: number) {
    let _message: string = "";
    if (code == 1) {
      _message = "É necessário um parâmetro para efetuar a busca, foneça o CEP ou ative a permissão de geolocalização!";
    } else {
      _message = "Erro ao recuperar coordenadas do gps!";
    }
    this.alertCtrl.create({
      title: "Erro",
      message: _message,
      enableBackdropDismiss: false,
      inputs: [{
        name: "cep",
        type: 'tel',
        max: 9,
        placeholder: "somente números"
      }],
      buttons: [{
        text: 'Efetuar busca por cep',
        handler: (form) => {
          this.findLocationByCep(form.cep);
        }
      },
      {
        text: "Ativar gps",
        handler: () => { if (code == 1) this.tryPermissionAgain(); else this.loadLojas(); }
      }]
    }).present();
  }

  findLocationByCep(cep: any) {
    this.utils.showLoader("buscando...");
    if (cep) {
      this.geoProvider.getGeolocationByAdress(cep)
        .subscribe(data => {
          if (data.status == "OK" && data.results && data.results.length) {
            this.getLojas(data.results[0].geometry.location.lat, data.results[0].geometry.location.lng);
          } else {
            this.utils.hideLoader();
            this.utils.showToast("Cep não válido");
            this.showRequestAlert(1);
          }
        });

    } else {
      this.utils.hideLoader();
      this.utils.showToast("opção inválida");
      this.showRequestAlert(1);
    }
  }

  getLojas(lat: number, lng: number) {
    this.storeProvider.lojas(lat, lng)
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

  getLojasPorPessoaId(lat: number, lng: number) {
    this.storeProvider.lojasUsuario(this.usuarioLojista, lat, lng)
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

  entrar(loja) {
    StoreProvider.setCurrentStore(loja);

    if (this.isModal) {
      this.viewCtrl.dismiss();
      return;
    }

    if (this.telaOrigem && this.telaOrigem == "MainStorePage")
      MainStorePage.ref.showTab(this.tabOrigem);
    else if (this.telaOrigem && this.telaOrigem == "MainAdminPage")
      MainAdminPage.ref.showTab(this.tabOrigem);
    else if (this.telaOrigem)
      this.navCtrl.push(this.telaOrigem, { "loja": loja });
  }

  cadastrarLoja() {
    if (this.isModal)
      this.viewCtrl.dismiss();

    this.navCtrl.push("StoreRegisterFullPage");
  }

}
