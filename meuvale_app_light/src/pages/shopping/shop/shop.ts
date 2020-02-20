import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ActionSheetController, ModalController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoreProvider } from '../../../providers/store/store';
import { Observable } from 'rxjs';
import { ProductProvider } from '../../../providers/product/product';
import { auth } from 'firebase';
import { HttpProvider } from '../../../providers/http/http';
import { PageProvider } from '../../../providers/page/page';

@IonicPage()
@Component({
  selector: 'page-shop',
  templateUrl: 'shop.html'
})
export class ShopPage {

  categorias: any;
  loja: any;

  Imagens = [{ URL: '' }]

  nomeLoja: string;
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
  lojista: boolean = false;
  public static lojaMaster: boolean = false;
  produtosLoja: boolean = false;
  telaOrigem = "MainStorePage";
  tabOrigem = 2;

  constructor(
    public navCtrl: NavController,
    private utils: UtilsProvider,
    private mdlCtrl: ModalController,
    private productProvider: ProductProvider,
    public navParams: NavParams,
    private actSheetCtrl: ActionSheetController) {

    this.loja = this.navParams.get("loja");
    if (!this.loja) {
      PageProvider.setCurrentStore(this.telaOrigem, this.tabOrigem);

      StoreProvider.storeChange.subscribe(
        data => {  }
      );
    }
    if (this.navParams.get("visao"))
      this.lojista = this.navParams.get("visao") ? this.navParams.get("visao") == "lojista" ? true : false : false;

    if (this.navParams.get("lojista") || HttpProvider.userAuth.PerfilId == '2')
      this.produtosLoja = HttpProvider.userAuth.PerfilId == '2' || this.navParams.get("lojista");

    if (ShopPage.lojaMaster == true)
      this.lojista = ShopPage.lojaMaster;
  }

  ionViewWillEnter() {
    this.loadData();
  }

  loadData(): any {
    this.loja = StoreProvider.getCurrentStore();
    if (!this.loja)
      this.escolherLojas(true);
    else {
      this.nomeLoja = this.loja.Descricao;
      this.loadProdutos();
    }
  }

  escolherLojas(force: boolean = false) {
    if (HttpProvider.userAuth.PerfilId == "99")
      this.telaOrigem = "MainAdminPage";

    let mdl = this.mdlCtrl.create("StoreSelectPage", { modal: true, noClose: force, "usuarioLojista": HttpProvider.userAuth.PessoaId, "telaOrigem": this.telaOrigem, "tabOrigem": this.tabOrigem });
    mdl.onDidDismiss(
      data => {
        if (data) {
          this.loadProdutos();
        }
      }
    )
    mdl.present();
  }

  detalhes(produtoMv) {
    let params = {
      loja: this.loja,
      produto: produtoMv
    };
    this.navCtrl.push('ProductDetailPage', params);
  }

  showOrder(): void {
    this.actSheetCtrl.create({
      title: "Selecione...",
      buttons: [
        { text: "Maior Preço", handler: () => this.orderTo(this.Order.MAIOR_PRECO) },
        { text: "Menor Preço", handler: () => this.orderTo(this.Order.MENOR_PRECO) },
        { text: "Melhor desconto", handler: () => this.orderTo(this.Order.MELHOR_DESCONTO) },
        { text: "A - Z", handler: () => this.orderTo(this.Order.A_Z) },
        { text: "Z - A", handler: () => this.orderTo(this.Order.Z_A) }
      ]
    }).present();
  }

  showFilter() {
    let mdl = this.mdlCtrl.create("ProductFilterPage", { "filter": this.filter });
    mdl.onDidDismiss(
      data => {
        if (data) {
          this.filter = data;
          this.doFilter();
        } else {
          this.filter = {};
          this._page = 0;
          this.loadProdutos();
        }
      }
    );
    mdl.present();
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
        return ele1.Valor - ele2.Valor;
      });
    }

    if (this.orderType == this.Order.MAIOR_PRECO) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return ele2.Valor - ele1.Valor;
      });
    }

    if (this.orderType == this.Order.MELHOR_DESCONTO) {
      this.produtos.sort((ele1: any, ele2: any) => {
        return ele1.ValorDesconto - ele2.ValorDesconto;
      });
    }
  }

  doFilter() {
    this._page = 0;
    this._doFilter();
  }

  _doFilter(infit: any = null) {

    if (!infit)
      this.utils.showLoader("carregando...");

    let listData: Observable<any>;

    if (this.filter && this.filter.departamentSelected) {
      listData = this.productProvider.listProdutoPorDepartamento(this.filter.departamentSelected.Id, this._page)
    }
    else {
      listData = this.productProvider.getProdutosFornecedor(this.loja.Id, this._page)
    }

    listData.subscribe(
      data => {
        if (infit) {
          if (data)
            this.produtos = this.produtos.concat(data);
        } else {
          this.produtos = data || [];
        }
        this.hasMore = data.length >= 10;
        this.utils.hideLoader();
        if (this.produtos.length == 0)
          this.utils.showAlert("atenção", "Não foram encontrados produtos");
        else
          this.doOrder();

        if (infit) infit.complete();

      }, error => {
        this.utils.hideLoader();
        this.utils.showToast(error.error);
        if (infit) infit.complete();
      }
    );
  }

  loadProdutos(infit: any = null) {
    if (!infit)
      this.utils.showLoader('carregando...');

    if (this.produtosLoja == false)
      this.productProvider.getProdutosFornecedor(this.loja.Id, this._page)
        .subscribe(
          data => {
            if (infit) {
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
            if (infit) infit.complete();
          }
        )
    else
      this.productProvider.getProdutosLoja(this.loja.Id, this._page)
        .subscribe(
          data => {
            if (infit) {
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
            if (infit) infit.complete();
          }
        )
  }

  doInfinite(infiniteScroll) {
    this._page += 10;
    console.log("_page = " + this._page);
    if (this.filter)
      this._doFilter(infiniteScroll);
    else
      this.loadProdutos(infiniteScroll);
  }

  goToProductRegister(product) {
    let params = {
      loja: this.loja,
      produto: product,
      lojista: this.produtosLoja
    };

    this.navCtrl.push("ProductRegisterPage", params)
  }
}
