import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController, Tabs, Platform } from 'ionic-angular';
import { StoreProvider } from '../../providers/store/store';
import { InAppBrowser, InAppBrowserOptions } from '@ionic-native/in-app-browser';
import { PaymentProvider } from '../../providers/payment/payment';
import { UtilsProvider } from '../../providers/utils/utils';
import { HttpProvider } from '../../providers/http/http';
import { AppVersion } from '@ionic-native/app-version/ngx';

@IonicPage()
@Component({
  selector: 'page-main-home',
  templateUrl: 'main-home.html',
})
export class MainHomePage {

  banners: any;
  produtos: any[] = [];
  produtoPesquisa: any;
  proudtosFilter: any[] = [];
  limparBusca: boolean = false;
  redirecionaPremmiar: boolean = false;

  @ViewChild("tabs") tabs: Tabs;
  private static _ref: MainHomePage = null;
  options: InAppBrowserOptions = {
    location: 'yes',//Or 'no' 
    hidden: 'no', //Or  'yes'
    clearcache: 'yes',
    clearsessioncache: 'yes',
    zoom: 'yes',//Android only ,shows browser zoom controls 
    hardwareback: 'yes',
    mediaPlaybackRequiresUserAction: 'no',
    shouldPauseOnSuspend: 'no', //Android only 
    closebuttoncaption: 'Close', //iOS only
    disallowoverscroll: 'no', //iOS only 
    toolbar: 'yes', //iOS only 
    enableViewportScale: 'no', //iOS only 
    allowInlineMediaPlayback: 'no',//iOS only 
    presentationstyle: 'pagesheet',//iOS only 
    fullscreen: 'yes',//Windows only    
  };

  constructor(public navCtrl: NavController,
    private inAppBrowser: InAppBrowser,
    private storeProvider: StoreProvider,
    private payment: PaymentProvider,
    private utils: UtilsProvider,
    private platform: Platform,
    private theInAppBrowser: InAppBrowser,
    private appVersion: AppVersion) {
    MainHomePage._ref = this;

    // if (this.platform.is('ios') || this.platform.is('android')) {
    //   this.appVersion.getAppName().then(value => {
    //     utils.showAlert('name', value)
    //   }).catch(err => {
    //     alert(err);
    //   });

    //   this.appVersion.getVersionCode().then(value => {
    //     utils.showAlert('versioncode', value.toString())
    //   }).catch(err => {
    //     alert(err);
    //   });

    //   this.appVersion.getVersionNumber().then(value => {
    //     utils.showAlert('versionnumber', value.toString())
    //   }).catch(err => {
    //     alert(err);
    //   });
    // }
  }

  ionViewDidLoad() {
    if (StoreProvider.getCurrentStore()) {
      if (this.tabs) {
        this.tabs.resize();
      }
    }

    StoreProvider.storeChange.subscribe(
      () => {
        if (this.tabs)
          this.tabs.resize();
      }
    )
  }

  public static get ref(): MainHomePage {
    return MainHomePage._ref;
  }

  public showTab(index: number): void {
    if (this != undefined
      && this.tabs != undefined
      && this.tabs.length != undefined)
      this.tabs.select(index);
  }

  gotoQrCodePay() {
    this.navCtrl.push("QrcodePayPage");
  }

  gotoRedeCredenciada() {
    this.navCtrl.push("StoreListPage");
  }

  gotoMinhasPromocoes() {
    this.inAppBrowser.create('https://meuclube.convenia.com.br/', '_blank', 'location=yes');
  }
  gotoMinhasCompras() {
    this.redirecionaPremmiar = true;
    this.validaCartaoAdiantamento();
  }
  gotoCartao() {
    this.navCtrl.push("CartaoListPage");
  }
  gotoAddCredenciado() {
    this.navCtrl.push("StoreIndicatePage");
  }
  gotoOdonto() {
    this.navCtrl.push("OdontoMktPage");
  }
  gotoConta() {
    this.navCtrl.push("DigitalAccountMktPage");
  }

  facebook() {
    //https://www.facebook.com/MeuValeBeneficios/
    let browser = 'https://www.facebook.com/MeuValeBeneficios/';
    let target = "_blank"; //_system //_self
    this.theInAppBrowser.create(browser, target, this.options);
  }

  contact() {
    this.navCtrl.push("ContactPage");
  }

  private validaCartaoAdiantamento() {
    this.payment.loadCartoes()
      .subscribe(
        data => {
          if (data.length > 0 && this.redirecionaPremmiar == true) {
            if (data.find(x => x.TipoCartao.AdiantamentoSalarial)) {
              this.redirecionaPremmiar = false;
              this.storeProvider.temAcesso(HttpProvider.userAuth.UsuarioId)
                .subscribe(resp => {
                  var location = resp.headers.get("Location");
                  if (location) {
                    this.inAppBrowser.create(location, '_blank', 'location=yes');
                  }
                },
                  error => {
                    this.utils.showToast("Erro ao acessar...");
                  });
            }
            else
              this.utils.showAlert("Sem permissão!", "Área exclusiva para clientes do cartão de Adiantamento Salárial");
          }
          else if (data.length == 0)
            this.utils.showAlert("Sem permissão!", "Área exclusiva para clientes do cartão de Adiantamento Salárial")
        }, error => {
          this.utils.showToast("Erro ao acessar...");
        }
      )
  }
}
