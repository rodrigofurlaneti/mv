import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController, NavParams, Slides } from 'ionic-angular';
import { BarcodeScanner, BarcodeScannerOptions } from '@ionic-native/barcode-scanner';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { StoreProvider } from '../../../providers/store/store';
import { MyApp } from '../../../app/app.component';

/**
 * Generated class for the QrcodePayPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-qrcode-pay',
  templateUrl: 'qrcode-pay.html',
})
export class QrcodePayPage {

  @ViewChild(Slides) slides: Slides;

  barcodeOptions: BarcodeScannerOptions;
  loja: any;
  tutorial: any;
  exibeTutorialQrCode = MyApp.exibeTutorialQrCode;
  exibeTutorialQrCodeCss = MyApp.exibeTutorialQrCode;

  constructor(public navCtrl: NavController,
    private barcodeScanner: BarcodeScanner,
    private utils: UtilsProvider,
    private shoppingProvider: ShoppingProvider,
    private storeProvider: StoreProvider,
    public navParams: NavParams) {
    this.loja = this.navParams.get("loja");
  }

  ionViewDidLoad() {
    if (this.exibeTutorialQrCode)
      this.storeProvider.getTutorial()
        .subscribe(data =>
          this.tutorial = data.filter(x => x.Id >= 5 && x.Id <= 10)
        );
  }

  click() {
    if (this.slides.getActiveIndex() == 4)
      this.exibeTutorialQrCode = false;

    this.slides.slideNext();
  }

  showBarcodeScan() {
    this.barcodeOptions = {
      prompt: 'Escaneie o QRCode para realizar o pagamento ao estabelecimento!'
    };

    this.barcodeScanner.scan(this.barcodeOptions).then(
      (barcodeData) => {
        if (barcodeData.cancelled) {
          this.utils.showToast("Escaneamento cancelado.");
          return;
        }

        let idLoja = parseInt(barcodeData.text);
        this.storeProvider.getLoja(idLoja).subscribe(
          data => {
            this.loja = data;
            this.loadLista();
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )
      }, (err) => {
        this.utils.showToast(err);
      });
  }

  loadLista() {
    if (HttpProvider.userAuth)
      this.shoppingProvider.getListaAtivaLoja(this.loja)
        .subscribe(
          data => {
            this.utils.hideLoader();
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )
    else
      this.shoppingProvider.getListaTemporaria(this.loja.Id)
        .subscribe(
          data => {
            this.utils.hideLoader();
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )

    this.navCtrl.push('OrderPaymentNewPage', { "loja": this.loja });
  }

  doacao() {
    this.storeProvider.getLoja(3466).subscribe(
      data => {
        this.loja = data;
        this.loadLista();
      },
      error => {
        this.utils.hideLoader();
        this.utils.showError(error.message)
      });
  }

}
