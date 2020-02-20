import { Component } from '@angular/core';
import { IonicPage, NavController} from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-main-agente',
  templateUrl: 'main-agente.html',
})
export class MainAgentePage {
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
    this.navCtrl.push("DocumentSelfiePage");
  }
}
