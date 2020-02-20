import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ConvenioProvider } from '../../../providers/convenio/convenio';
/**
 * Generated class for the ConvenioListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-convenio-list',
  templateUrl: 'convenio-list.html',
})
export class ConvenioListPage {

  _page: number = 0;
  lojas: any[] = [];

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private convenioProvider: ConvenioProvider) {
  }

  ionViewWillEnter() {
    this.loadLojas();
  }

  loadLojas(concat: boolean = false) {
    this.utils.showLoader("localizando convênios");
    this.getLojas(concat);
  }

  getLojas(concat: boolean = true) {
    this.convenioProvider.getAll()
      .subscribe(
        data => {
          if (concat == true)
            this.lojas = this.lojas.concat(data);
          else
            this.lojas = data;
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        },
        () => this.utils.hideLoader()
      );
  }

  edit(loja) {
    this.navCtrl.push('ConvenioRegisterPage', { "loja": loja });
  }

  trash(plano) {
    this.convenioProvider.delete(plano)
      .subscribe(
        data => {
          var index = this.lojas.indexOf(plano);
          this.lojas.splice(index, 1);
        }, error => {
          this.utils.showToast(error.error);
        }
      );
  }

  register() {
    this.navCtrl.push('ConvenioRegisterPage');
  }

  doInfinite(infiniteScroll) {
    this._page += 50;
    setTimeout(() => {
      this.getLojas(true);
      infiniteScroll.complete();
    }, 500);
  }
}
