import { Component, ViewChild } from '@angular/core';
import { IonicPage, Tabs } from 'ionic-angular';
import { StoreProvider } from '../../providers/store/store';
import { ShopPage } from '../shopping/shop/shop';

@IonicPage()
@Component({
  selector: 'page-main-partner',
  templateUrl: 'main-partner.html'
})

export class MainPartnerPage {

  tab1Root: any = "PartnerStoreListPage";
  @ViewChild("tabs") tabs: Tabs;
  private static _ref: MainPartnerPage = null;

  constructor() {
    MainPartnerPage._ref = this;
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
  public static get ref(): MainPartnerPage {
    return MainPartnerPage._ref;
  }

  public showTab(index: number): void {
    if (this != undefined
      && this.tabs != undefined
      && this.tabs.length != undefined)
        this.tabs.select(index);
  }

}
