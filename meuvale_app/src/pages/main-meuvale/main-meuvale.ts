import { Component, ViewChild } from '@angular/core';
import { IonicPage, Tabs } from 'ionic-angular';
import { StoreProvider } from '../../providers/store/store';

@IonicPage()
@Component({
  selector: 'page-main-meuvale',
  templateUrl: 'main-meuvale.html'
})

export class MainMeuValePage {

  tab1Root: any = "CartaoListPage";
  tab2Root: any = "QrcodePayPage";
  tab3Root: any = "StoreListPage";
  tab4Root: any = "ShopClientPage";
  private static _ref: MainMeuValePage = null;
  @ViewChild("tabs") tabs: Tabs;
  
  constructor() {
    MainMeuValePage._ref = this;
  }
  
  ionViewDidLoad() {
    if(StoreProvider.getCurrentStore())
	  {
      if (this.tabs){
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

  public static get ref(): MainMeuValePage {
    return MainMeuValePage._ref;
  }

  public showTab(index: number): void {
	  if (this !=  undefined 
      && this.tabs != undefined 
      && this.tabs.length != undefined)
		  this.tabs.select(index);
  }
}
