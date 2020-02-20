import { Component } from '@angular/core';
import { IonicPage, NavController, Platform } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-download',
  templateUrl: 'download.html',
})
export class DownloadPage {

  constructor(public navCtrl: NavController,
    private platform: Platform) {
  }

  ionViewDidLoad() {
    if (this.platform.is('ios'))
      window.open('https://play.google.com/store/apps/details?id=br.com.grupormc.meuvaleapp', '_system');
    else (this.platform.is('android'))
      window.open('https://play.google.com/store/apps/details?id=br.com.grupormc.meuvaleapp', '_system');
  }

}
