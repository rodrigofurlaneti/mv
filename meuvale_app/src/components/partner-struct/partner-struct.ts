import { Component, OnInit, Input } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ProductProvider } from '../../providers/product/product';
import { UtilsProvider } from '../../providers/utils/utils';
import { ProductLoja } from '../../model/product-loja';

/**
 * Generated class for the PartnerStructComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */

@Component({
  selector: 'partner-struct',
  templateUrl: 'partner-struct.html'
})
export class PartnerStructComponent implements OnInit {

  @Input("categoria-oferta") private categoriaOferta: string;

  produtos: ProductLoja[] = [];
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
    if (this.categoriaOferta)
    {
      this.productProvider.listOfertasPorCategoria(parseInt(this.categoriaOferta), page)
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
    else 
    {
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
  }

  detalhes(produtoMv) {
    let params = {
      produto: produtoMv
    };
    this.navCtrl.push('CupomDetailPage', params);
  }
}
