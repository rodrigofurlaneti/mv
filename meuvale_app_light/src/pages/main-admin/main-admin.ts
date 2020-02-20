import { Component, ViewChild } from '@angular/core';
import { IonicPage, Tabs } from 'ionic-angular';
import { StoreProvider } from '../../providers/store/store';

@IonicPage()
@Component({
  selector: 'page-main-admin',
  templateUrl: 'main-admin.html'
})

export class MainAdminPage {

  tab1Root: any = "OursStoreListPage";
  tab2Root: any = "PartnerStoreListPage";
  tab3Root: any = "OrderStoreListPage";
  @ViewChild("tabs") tabs: Tabs;
  private static _ref: MainAdminPage = null;
  
  constructor() {
    MainAdminPage._ref = this;
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
  public static get ref(): MainAdminPage {
    return MainAdminPage._ref;
  }

  public showTab(index: number): void {
	  if (this !=  undefined 
      && this.tabs != undefined 
      && this.tabs.length != undefined)
		  this.tabs.select(index);
  }

}
