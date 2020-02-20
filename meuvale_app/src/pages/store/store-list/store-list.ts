import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController, AlertController, Platform } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';
import { Geolocation, PositionError } from '@ionic-native/geolocation';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoresMapComponent } from '../../../components/stores-map/stores-map';
import { Diagnostic } from '@ionic-native/diagnostic/ngx';
import { GeolocationProvider } from '../../../providers/google/geolocation';
/**
 * Generated class for the StoreListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-store-list',
  templateUrl: 'store-list.html',
})
export class StoreListPage {

  fechado: boolean = false;
  _page: number = 0;
  lat: any;
  log: any;
  range: number = 50;

  @ViewChild("map") map: StoresMapComponent;
  lojas: any[] = [];

  //produtos: ProductLoja[] = [];
  lojaPesquisa: string;
  lojasFilter: any[] = [];
  limparBusca: boolean = false;

  constructor(public navCtrl: NavController,
    private geo: Geolocation,
    private diagnostic: Diagnostic,
    private geoProvider: GeolocationProvider,
    private platform: Platform,
    private utils: UtilsProvider,
    private alertCtrl: AlertController,
    private storeProvider: StoreProvider) {
  }

  ionViewDidLoad() {
    this.loadLojas();
  }

  loadLojas(concat: boolean = true) {
    this.utils.showLoader("localizando lojas");
    this.geo.getCurrentPosition({
      timeout: 30000,
      enableHighAccuracy: true
    }).then(
      resp => {
        if (resp && resp.coords)
          this.getLojas(resp.coords.latitude, resp.coords.longitude, concat);
        else {
          this.utils.hideLoader();
          this.utils.showToast("erro ao consultar o gps");
        }
      }
    ).catch((pe: PositionError) => {
      this.utils.hideLoader();
      this.showRequestAlert(pe.code);
    });
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
            this.map.goByLatLong(data.results[0].geometry.location.lat, data.results[0].geometry.location.lng);
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

  getLojas(lat: number, lng: number, concat: boolean = true) {
    if (lat)
      this.lat = lat;
    if (lng)
      this.log = lng;

    this.storeProvider.lojas(this.lat, this.log, this._page, this.range)
      .subscribe(
        data => {
          if (concat == true)
            this.lojas = this.lojas.concat(data);
          else
            this.lojas = data;

          let markers: any[] = [];
          for (var i = 0; i < this.lojas.length; i++) {
            var obj = {
              nome: this.lojas[i].Descricao,
              lat: parseFloat(this.lojas[i].Endereco.Latitude),
              lng: parseFloat(this.lojas[i].Endereco.Longitude)
            };
            markers.push(obj);
          }
          this.map.showMarks(markers);
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        },
        () => this.utils.hideLoader()
      );
  }

  entrar(loja) {
    this.navCtrl.push('StoreDetailPage', { "loja": loja });
  }

  doInfinite(infiniteScroll) {
    this._page += 50;
    setTimeout(() => {
      this.getLojas(null, null);
      infiniteScroll.complete();
    }, 500);
  }

  search() {
    if (!this.lojaPesquisa || this.lojaPesquisa == "")
      this.loadLojas();

    this.storeProvider.getTipoEstabelecimentoPorNome(this.lojaPesquisa)
      .subscribe(
        data => {
          this.lojas = data;
          this.storeProvider.getLojasPorNome(this.lojaPesquisa, 0)
            .subscribe(
              lojasPorNome => {
                this.lojas = this.lojas.concat(lojasPorNome);
              },
              error => {
                console.log(error);
                this.utils.showError(error.message);
              }
            )
        },
        error => {
          console.log(error);
          this.utils.showError(error.message);
        }
      )
  }

  limparBuscaLojas() {
    this.lojas = [];
    this.loadLojas();
    this.limparBusca = false;
  }


  rangeChanged(qtd) {
    this.range = qtd;
    this._page = 0;
    this.loadLojas(false);
  }
}
