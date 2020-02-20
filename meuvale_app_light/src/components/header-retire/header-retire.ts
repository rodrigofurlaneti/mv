import { Component, OnInit, NgZone } from '@angular/core';
import { StoreProvider } from '../../providers/store/store';
import { ModalController, NavController } from 'ionic-angular';
import { HttpProvider } from '../../providers/http/http';
import { PageProvider } from '../../providers/page/page';

/**
 * Generated class for the HeaderRetireComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
@Component({
  selector: 'header-retire',
  templateUrl: 'header-retire.html'
})
export class HeaderRetireComponent implements OnInit {

  title: string = '';
  isVisible: boolean = false;
  
  constructor(public navCtrl: NavController,
    private mdl: ModalController,
    private ngZoone: NgZone) { }

  ngOnInit(): void {
    StoreProvider.storeChange.subscribe(
      data => this.loadTitle(data)
    );
    this.loadTitle(StoreProvider.getCurrentStore());
  }

  private loadTitle(data): void {
    this.ngZoone.run(
      zooner => {
        this.title = data ? data.Descricao : null;
        this.isVisible = !!this.title;
      }
    );
  }

  trocarLoja() {
    this.mdl.create("StoreSelectPage", { 
      "usuarioLojista": HttpProvider.userAuth.PessoaId, 
      "telaOrigem": PageProvider._pageSelected,
      "tabOrigem": PageProvider._tabSelected, 
      modal: true,
      noClose: true })
      .present();
  }
}
