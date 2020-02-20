import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { BarcodeScanner, BarcodeScannerOptions } from '@ionic-native/barcode-scanner';
import { UtilsProvider } from '../../../providers/utils/utils';
import { OrderProvider } from '../../../providers/order/order';

/**
 * Generated class for the OrderWithdrawPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-order-withdraw',
  templateUrl: 'order-withdraw.html',
})
export class OrderWithdrawPage {

  retirarPedido: any;
  barcodeOptions: BarcodeScannerOptions;

  constructor(public navCtrl: NavController, 
    private barcodeScanner: BarcodeScanner,
    private orderProvider: OrderProvider,
    private utils: UtilsProvider,
    public navParams: NavParams) {
  }

  showBarcodeScan() {
    // this.orderProvider.getPedidoVoucher(1).subscribe(data => {
    //   this.utils.showAlert("Sucesso", "Voucher validado com sucesso!");
    // });

    this.barcodeOptions = {
      prompt: 'Escaneie o QRCode para realizar o resgate do voucher.'
    };

    this.barcodeScanner.scan(this.barcodeOptions).then((barcodeData) => {

      if (barcodeData.cancelled) {
        this.utils.showToast("Escaneamento cancelado.");
        return;
      }

      let text = barcodeData.text;
      this.orderProvider.getPedidoVoucher(parseInt(text)).subscribe(
        data => {
        if (!data) {
          this.utils.showToast("Voucher não encontrado!");
        } else {
          this.utils.showAlert("Sucesso", "Voucher validado com sucesso!");
        }
      }, error => {
        this.utils.showToast(error.message);
      });
    }, (err) => {
        this.utils.showToast(err.message);
    });
  }

}
