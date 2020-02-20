import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { BarcodeScanner, BarcodeScannerOptions } from '@ionic-native/barcode-scanner';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { StoreProvider } from '../../../providers/store/store';

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

  barcodeOptions: BarcodeScannerOptions;
  loja: any;

  constructor(public navCtrl: NavController,
    private barcodeScanner: BarcodeScanner,
    private utils: UtilsProvider,
    private shoppingProvider: ShoppingProvider,
    private storeProvider: StoreProvider,
    public navParams: NavParams) {
      this.loja = this.navParams.get("loja");
  }

  ionViewDidLoad() {

  }

  showBarcodeScan() {
    this.barcodeOptions = {
      prompt: 'Escaneie o QRCode para realizar o pagamento ao estabelecimento!'
    };

    this.barcodeScanner.scan(this.barcodeOptions).then(
      (barcodeData) => {
        console.log(barcodeData);
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
        this.storeProvider.getLoja(19401).subscribe(
          data => {
            this.loja = data;
            this.loadLista();
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )
      });
  }

  loadLista() {
    if (HttpProvider.userAuth)
      this.shoppingProvider.getListaAtivaLoja(this.loja)
        .subscribe(
          data => {
            this.utils.hideLoader();
            this.navCtrl.push('OrderPaymentNewPage', { "loja": this.loja });
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
            this.navCtrl.push('OrderPaymentNewPage', { "loja": this.loja });
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )
  }

}
