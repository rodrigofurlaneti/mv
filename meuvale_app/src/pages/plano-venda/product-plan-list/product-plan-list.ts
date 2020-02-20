import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ProductPlanProvider } from '../../../providers/product-plan/product-plan';

/**
 * Generated class for the ProductPlanListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-product-plan-list',
  templateUrl: 'product-plan-list.html',
})
export class ProductPlanListPage {

  _page: number = 0;
  planos: any[] = [];

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private productPlanProvider: ProductPlanProvider) {
  }

  ionViewWillEnter() {
    this.loadLojas();
  }

  loadLojas(concat: boolean = false) {
    this.utils.showLoader("localizando combos");
    this.getLojas(concat);
  }

  getLojas(concat: boolean = false) {
    this.productPlanProvider.getAll()
      .subscribe(
        data => {
          if (concat == true)
            this.planos = this.planos.concat(data);
          else
            this.planos = data;
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        },
        () => this.utils.hideLoader()
      );
  }

  edit(plano) {
    this.navCtrl.push('ProductPlanRegisterPage', { "planoVenda": plano });
  }

  trash(plano) {
    this.productPlanProvider.delete(plano)
      .subscribe(
        data => {
          var index = this.planos.indexOf(plano);
          this.planos.splice(index, 1);
        }, error => {
          this.utils.showToast(error.error);
        }
      );
  }

  register() {
    this.navCtrl.push('ProductPlanRegisterPage');
  }

  doInfinite(infiniteScroll) {
    this._page += 50;
    setTimeout(() => {
      this.getLojas(true);
      infiniteScroll.complete();
    }, 500);
  }
}
