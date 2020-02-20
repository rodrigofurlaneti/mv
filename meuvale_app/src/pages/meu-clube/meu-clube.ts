import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { UtilsProvider } from '../../providers/utils/utils';
import { FornecedorProvider } from '../../providers/fornecedor/fornecedor';
import { StoreProvider } from '../../providers/store/store';
import { InAppBrowser, InAppBrowserOptions } from '@ionic-native/in-app-browser';

/**
 * Generated class for the MeuClubePage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-meu-clube',
  templateUrl: 'meu-clube.html',
})
export class MeuClubePage {

  fornecedores:  any[] = [];
  _page: number = 0;
  hasMore: boolean;
  hasMoreFornecedor: boolean = true;

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private fornecedorProvider: FornecedorProvider,
    private storeProvider: StoreProvider,
    private inAppBrowser: InAppBrowser,
    public navParams: NavParams) {
  }

  ionViewDidLoad() {
    this.loadLojas();
  }

  loadLojas(infit: any = null) {
    if (this.hasMoreFornecedor)
    this.fornecedorProvider.fornecedoresPorClassificacao(0, 0, this._page, 10, "MeuClube")
      .subscribe(
        data => { 
          this.fornecedores = this.fornecedores.concat(data); 
          this.hasMoreFornecedor = data.length >= 5;
          this.storeProvider.lojasPorClassificacao(0, 0, this._page, data.length >= 5 ? 5 : 5 + (5 - data.length), "MeuClube")
          .subscribe(
            data => { 
              this.fornecedores = this.fornecedores.concat(data); 
               if(infit) infit.complete();
              this.hasMore = data.length >= 10;
            },
            error => { this.utils.showToast(error.error); 
              if(infit) infit.complete();}
          );
        },
        error => { this.utils.showToast(error.error); 
          if(infit) infit.complete();}
      );
      else
      this.storeProvider.lojas(0, 0, this._page, 10)
          .subscribe(
            data => { 
              this.fornecedores = this.fornecedores.concat(data); 
               if(infit) infit.complete();
              this.hasMore = data.length >= 10;
            },
            error => { this.utils.showToast(error.error); 
              if(infit) infit.complete(); }
          );
  }

  detalhes(loja) {
    this.navCtrl.push("ShopPage", { loja: loja, "lojista" : loja.TipoEstabelecimento == "LOJA" });
  }

  doInfinite(infiniteScroll) {
    this._page += 5;
    setTimeout(() => {
      this.loadLojas(infiniteScroll);
    }, 500);
  }
}
