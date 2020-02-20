import { Component } from '@angular/core';
import { IonicPage, NavController, Keyboard } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';
import { HttpProvider } from '../../../providers/http/http';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PeopleProvider } from '../../../providers/people/people';

@IonicPage()
@Component({
  selector: 'store-list-qrcode',
  templateUrl: 'store-list-qrcode.html',
})
export class StoreListQrCodesPage {

  _qtd: number = 10000;
  _page: number = 0;
  _pages: number[] = [];
  lojas: any[] = [];
  lojasPaginadas: any[] = [];
  lojaPesquisa: string = "";
  qtdPesquisa: number = 1;
  showMore: boolean = false;
  limparBusca: boolean = false;
  paisSelected: number = 0;
  estadoSelected: number = 0;
  cidadeSelected: number = 0;
  paises: any[] = [];
  estados: any[] = [];
  cidades: any[] = [];
  cidadesFilter: any[] = [];
  cidadeDescricao: string;
  cidadeBusca: string;
  bairro: string = "";

  constructor(public navCtrl: NavController,
    private utils: UtilsProvider,
    private peopleProvider: PeopleProvider,
    private keyboard: Keyboard,
    private storeProvider: StoreProvider) {
  }

  ionViewDidLoad() {
    // this.storeProvider.lojas(0, 0, 0, 50).subscribe(data => {
    //   this.lojas = data;

    //   this.lojasPaginadas = this.lojas.slice(0, 18);

    //   this.showMore = data.length >= this._qtd;
    //   this._pages = Array(Math.ceil(data.length / 18)).fill(0).map((x, i) => i);;
    // });

    this.loadData();
  }

  loadData() {
    this.peopleProvider.getPaises()
      .subscribe(
        data => {
          this.paises = data || [];
          if (this.paises.length) {
            this.paisSelected = this.paises[0].Id;
          }

          this.peopleProvider.getEstados()
            .subscribe(
              data => {
                this.estados = data || [];
                if (this.estadoSelected) {
                  this.peopleProvider.getCidades(this.estadoSelected)
                    .subscribe(
                      data => {
                        this.cidades = data || [];
                      },
                      error => this.utils.showToast(error)
                    )
                }
              },
              error => this.utils.showToast(error)
            );
        },
        error => this.utils.showToast(error)
      );
  }

  onChangeEstado() {
    console.log("cidades")
    this.peopleProvider.getCidades(this.estadoSelected)
      .subscribe(
        data => {
          this.cidades = data || [];
          this.cidadeBusca = '';
          this.cidadeDescricao = '';
          this.cidadeSelected = 0;
          this.cidadesFilter = null;
        },
        error => this.utils.showToast(error)
      )
  }

  searchCity() {
    if (this.estadoSelected == 0) {
      this.utils.showError('Selecione um estado para pesquisar uma cidade!');
      return;
    }

    if (!this.cidadeBusca.trim().length || !this.keyboard.isOpen()) {
      this.cidadesFilter = [];
      return;
    }

    this.cidadesFilter = this.cidades.filter(x => x.Descricao.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toUpperCase().includes(this.cidadeBusca.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toUpperCase()));
  }

  selecionaCidade(cidade){
    this.cidadeSelected = cidade.Id;
    this.cidadeBusca = cidade.Descricao;
    this.cidadesFilter = [];
  }

  removeFocus() {
    this.keyboard.close();
  }

  print() {
    if (window) {
      window.print();
    } else {
      console.warn("Não foi possível imprimir!");
    }
  }

  clickNext(): void {
    this._page += 1;

    this.lojasPaginadas = this.lojas.slice(this._page * 18, this._page * 18 + 18);
  }

  clickPrevious(): void {
    this._page -= 1;

    this.lojasPaginadas = this.lojas.slice(this._page * 18, this._page * 18 + 18);
  }

  limparBuscaLojas() {
    this._page = 0;
    this.lojas = [];
    this.search();
    this.limparBusca = false;
  }

  search(page: number = 0) {
    if (!this.lojaPesquisa || this.lojaPesquisa == "")
      this.lojas = [];

    this.utils.showLoader("buscando lojas...");
    this.storeProvider.getLojasPorNomeDocumentoOuCidade(this.estadoSelected, this.cidadeSelected, this.bairro, this.lojaPesquisa, this.qtdPesquisa, page * this._qtd, this._qtd)
      .subscribe(
        data => {
          this.utils.hideLoader();
          this.lojas = data;

          this.lojasPaginadas = this.lojas.slice(0, 18);

          this.showMore = data.length >= this._qtd;
          this._pages = Array(Math.ceil(data.length / 18)).fill(0).map((x, i) => i);;
        },
        error => {
          console.log(error);
          this.utils.hideLoader();
          this.utils.showError(error.message);
        }
      )
  }
}
