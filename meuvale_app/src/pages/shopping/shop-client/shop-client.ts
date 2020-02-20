import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ActionSheetController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoreProvider } from '../../../providers/store/store';
import { ProductProvider } from '../../../providers/product/product';
import { InAppBrowser } from '@ionic-native/in-app-browser';
//import { WebView } from '@ionic-native/ionic-webview/';
//import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@IonicPage()
@Component({
  selector: 'page-shop-client',
  templateUrl: 'shop-client.html'
})
export class ShopClientPage {

  produtos: any[] = [];
  
  orderType: number;
  Order: any = {
    MAIOR_PRECO: 1,
    MENOR_PRECO: 2,
    MELHOR_DESCONTO: 3,
    A_Z: 4,
    Z_A: 5
  }
  filter: any;
  _page: number = 0;
  hasMore: boolean;
  produtoPesquisa: any;
  proudtosFilter: any[] = [];
  limparBusca: boolean = false;
  //shoppingLink: SafeResourceUrl;

  constructor(
    public navCtrl: NavController,
    private utils: UtilsProvider,
    private productProvider: ProductProvider,
    public navParams: NavParams,
    private storeProvider: StoreProvider,
    private inAppBrowser: InAppBrowser,
    //public sanitizer: DomSanitizer,
    private actSheetCtrl: ActionSheetController) {
  }

  ionViewDidLoad() {
    // this.utils.showLoader("Carregando MeuClube...");
    // this.storeProvider.temAcesso()
    //   .subscribe(resp => {
    //     var location = resp.headers.get("Location");
    //     if(location)
    //     {
    //       //window.location.href = location;
    //       let browser = this.inAppBrowser.create(location, '_self', 'location=yes,zoom=no');
    //       //this.shoppingLink = this.sanitizer.bypassSecurityTrustResourceUrl(location);
    //     }
    //   },
    //   error => {
    //     console.log(error);
    //   });

    // this.utils.hideLoader();

    this.loadData();
    StoreProvider.storeChange.subscribe(
      data => {
        this._page = 0;
        this.loadData();
      }
    )
  }

  loadData(): any {
      this.loadProdutos();
  }

  detalhes(produtoMv) {
    let params = {
        produto: produtoMv
    };
    if (produtoMv.CategoriaProdutoId == 1)
    this.navCtrl.push('ProductDetailPage', params);
  }

  showOrder(): void {
    this.actSheetCtrl.create({
      title: "Selecione...",
      buttons: [        
        {text: "Maior Preço", handler: () => this.orderTo(this.Order.MAIOR_PRECO)},
        {text: "Menor Preço", handler: () => this.orderTo(this.Order.MENOR_PRECO)},
        {text: "Melhor desconto", handler: () => this.orderTo(this.Order.MELHOR_DESCONTO)},
        {text: "A - Z", handler: () => this.orderTo(this.Order.A_Z)},
        {text: "Z - A", handler: () => this.orderTo(this.Order.Z_A)}
      ]
    }).present();
  }

  private orderTo(type: number) {
    this.orderType = type;
    this.doOrder();
  }

  private doOrder(): any {
    if (this.orderType == this.Order.A_Z) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return ele1.Nome.localeCompare(ele2.Nome);
      });
    }

    if (this.orderType == this.Order.Z_A) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return -(ele1.Nome.localeCompare(ele2.Nome));
      });
    }

    if (this.orderType == this.Order.MENOR_PRECO) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return ele1.Preco - ele2.Preco;
      });
    }

    if (this.orderType == this.Order.MAIOR_PRECO) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return ele2.Preco - ele1.Preco;
      });
    }

    if (this.orderType == this.Order.MELHOR_DESCONTO) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return ele1.PrecoComDesconto - ele2.PrecoComDesconto;
      });
    }
  }
  
  loadProdutos(infit: any = null) {
    if (!infit)
      this.utils.showLoader('carregando...');

    this.productProvider.getProdutos(this._page)
      .subscribe(
        data => {
          if (infit)
            {
              this.produtos = this.produtos.concat(data);
              infit.complete();
            }
          else
            this.produtos = data;

          this.hasMore = data.length >= 10;
          this.utils.hideLoader();
        },
        error => {
          console.log(error);
          this.utils.showError(error.message);
          if(infit) infit.complete();
        }
      )
  }

  doInfinite(infiniteScroll) {
    this._page += 10;
    setTimeout(() => {
      this.loadProdutos(infiniteScroll);
    }, 500);
  }

  search() {
    if (!this.produtoPesquisa || this.produtoPesquisa == "")
      this.proudtosFilter = null;

    this.productProvider.getDepartamentosPorNome(this.produtoPesquisa)
      .subscribe(
        data => {
          this.proudtosFilter = data;
          this.productProvider.getProdutosPorNome(this.produtoPesquisa, 0)
            .subscribe(
              data => {
                this.proudtosFilter = this.proudtosFilter.concat(data);
              },
              error => {
                console.log(error);
                this.utils.showError(error.message);
              }
            )
        },
        error => {
          console.log(error);
          this.utils.showError(error.message);
        }
      )
  }

  retornaProdutos(produto) {
    this.productProvider.getProdutosPorNome(produto.Nome, 0)
      .subscribe(
        data => {
          this.produtos = data;
        },
        error => {
          console.log(error);
          this.utils.showError(error.message);
        }
      )

    this.proudtosFilter = null;
    this.produtoPesquisa = "";
    this.limparBusca = true;
  }

  limparBuscaProdutos(){
    this.produtos = [];
    this.loadProdutos(0);
    this.limparBusca = false;
  }
}
