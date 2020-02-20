import { Component } from '@angular/core';
import { IonicPage } from 'ionic-angular';
import { FornecedorProvider } from '../../../providers/fornecedor/fornecedor';

@IonicPage()
@Component({
  selector: 'page-parceiros',
  templateUrl: 'parceiros.html',
})
export class ParceirosPage {

  fornecedores: any;
  banners: any;
  descontos: any;

  constructor(
    private fornecedorProvider: FornecedorProvider
  ) {
  }

  ionViewDidLoad() {
    this.fornecedorProvider.list()
      .subscribe(data => this.fornecedores = data);

  }
}
