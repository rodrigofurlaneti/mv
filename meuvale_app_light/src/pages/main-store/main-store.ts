import { Component, ViewChild } from '@angular/core';
import { IonicPage, Tabs } from 'ionic-angular';
import { StoreProvider } from '../../providers/store/store';
import { ShopPage } from '../shopping/shop/shop';

@IonicPage()
@Component({
  selector: 'page-main-store',
  templateUrl: 'main-store.html'
})

export class MainStorePage {

  tab1Root: any = "OursStoreListPage";
  tab3Root: any = "OrderStoreListPage";
  tab2Root: any = "ShopPage";
  @ViewChild("tabs") tabs: Tabs;
  private static _ref: MainStorePage = null;

  constructor() {
    MainStorePage._ref = this;
  }

  ionViewDidLoad() {
    if (StoreProvider.getCurrentStore()) {
      if (this.tabs) {
        this.tabs.resize();
      }
    }

    StoreProvider.storeChange.subscribe(
      () => {
        if (this.tabs)
          this.tabs.resize();
      }
    )

    ShopPage.lojaMaster = true;
  }
  public static get ref(): MainStorePage {
    return MainStorePage._ref;
  }

  public showTab(index: number): void {
    if (this != undefined
      && this.tabs != undefined
      && this.tabs.length != undefined)
        this.tabs.select(index);
  }

}
