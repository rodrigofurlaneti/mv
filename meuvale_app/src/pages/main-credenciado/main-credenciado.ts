import { Component } from '@angular/core';
import { IonicPage, NavController} from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-main-credenciado',
  templateUrl: 'main-credenciado.html',
})
export class MainCredenciadoPage {
  constructor(public navCtrl: NavController) {
  }

  gotoConvenios() {
    this.navCtrl.push("ConvenioListPage");
  }
  
  gotoCombos() {
    this.navCtrl.push("ProductPlanListPage");
  }

  gotoCredenciado() {
    this.navCtrl.push("StoreRegisterFullPage");
  }

  gotoPlanos(){
    this.navCtrl.push("ProductPlanOrderPage");
  }
}
