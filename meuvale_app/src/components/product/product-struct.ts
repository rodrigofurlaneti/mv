import { Component, OnInit } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ProductProvider } from '../../providers/product/product';
import { UtilsProvider } from '../../providers/utils/utils';
import { ProductLoja } from '../../model/product-loja';

/**
 * Generated class for the ProductStructComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */

@Component({
  selector: 'product-struct',
  templateUrl: 'product-struct.html'
})
export class ProductStructComponent implements OnInit {

  produtos: ProductLoja[] = [];
  produtos2: ProductLoja[] = [];
  produtos3: ProductLoja[] = [];
  produtoPesquisa: string;
  proudtosFilter: ProductLoja[] = [];
  limparBusca: boolean = false;

  constructor(public navCtrl: NavController,
    private productProvider: ProductProvider,
    private utils: UtilsProvider) { }

  ngOnInit(): void {
    this.loadProdutos(0);
  }

  loadProdutos(page) {
    this.productProvider.getOfertas(page)
      .subscribe(
        data => {
          this.produtos = this.produtos.concat(data);
        },
        error => {
          console.log(error);
          this.utils.showError(error.message);
        }
      )
  }

  detalhes(produtoMv) {
    let params = {
      produto: produtoMv
    };
    this.navCtrl.push('ProductDetailPage', params);
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

  limparBuscaProdutos() {
    this.produtos = [];
    this.loadProdutos(0);
    this.limparBusca = false;
  }
}
