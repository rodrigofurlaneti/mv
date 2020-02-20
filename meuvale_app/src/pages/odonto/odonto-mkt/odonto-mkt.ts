import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { APPKEYS_CONFIG } from '../../../app/config/api.config';
import { ProductLoja } from '../../../model/product-loja';
import { ShoppingProvider } from '../../../providers/shopping/shopping';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ProductProvider } from '../../../providers/product/product';
import { HttpProvider } from '../../../providers/http/http';

/**
 * Generated class for the OdontoMktPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-odonto-mkt',
  templateUrl: 'odonto-mkt.html',
})
export class OdontoMktPage {

  loja = { Id: APPKEYS_CONFIG.credenciadoMeuVale };
  produtoMv: ProductLoja;
  
  constructor(public navCtrl: NavController,
    private shoppingProvider: ShoppingProvider,
    private utils: UtilsProvider,
    private productProvider: ProductProvider,
    public navParams: NavParams) {
  }

  ionViewDidLoad() {
    this.loadLista();
    this.loadProduto();
  }

  showForm() {
    this.navCtrl.push("ContactPage", { tipo: "PlanoOdonto" });
  }

  loadLista() {
    if (HttpProvider.userAuth)
      this.shoppingProvider.getListaAtivaLoja(this.loja)
        .subscribe(
          data => {
            this.utils.hideLoader();
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )
    else
      this.shoppingProvider.getListaTemporaria(this.loja.Id)
        .subscribe(
          data => {
            this.utils.hideLoader();
          },
          error => {
            this.utils.hideLoader();
            this.utils.showError(error.message)
          }
        )
  }

  loadProduto() {
    this.productProvider.getProdutoPorCodigo(APPKEYS_CONFIG.produtoBemMaisOdonto)
      .subscribe(
        data => {
          this.produtoMv = data;
        },
        error => {
          this.utils.showError(error.message);
        }
      )
  }

  pagar(){
    this.navCtrl.push('OrderPaymentNewPage', { "loja": this.loja, avaliacao: false, valor: this.produtoMv.ValorDesconto });
  }
}
