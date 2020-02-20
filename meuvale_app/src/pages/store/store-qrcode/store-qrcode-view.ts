import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { SocialSharing } from '@ionic-native/social-sharing';

/*
  Generated class for the StoreQrcodePage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/

@IonicPage()
@Component({
  selector: 'page-store-qrcode',
  templateUrl: 'store-qrcode-view.html'
})
export class StoreQrcodeViewPage {

  QRCode: string = "0";
  loja: any;
  lojas: any[] = [];

  constructor(public navCtrl: NavController,
    private socialSharing: SocialSharing, 
    public navParams: NavParams) 
  {
    this.loja = this.navParams.get("loja");

    this.QRCode = 
    this.loja.Id.toString();
    this.lojas.push(this.loja);
  }

  ionViewDidLoad() {
  }

  visualizar()
  {
    let params = {
      pet: this.loja
    };

    this.navCtrl.push("QrcodePayPage", params)
  }

//   private dataURItoBlob(dataURI) {
//     let binary = atob(dataURI.split(',')[1]);
//     let array = [];
//     for (let i = 0; i < binary.length; i++) {
//         array.push(binary.charCodeAt(i));
//     }
//     return new Blob([new Uint8Array(array)], { type: 'image/png' });
// }

  compartilhar(){
    
    this.socialSharing.share('','','',"http://meuvale.com.br/");
  }

  print() {
    if (window) {
      window.print();
    } else {
      console.warn("Não foi possível imprimir!");
    }
  }

}
