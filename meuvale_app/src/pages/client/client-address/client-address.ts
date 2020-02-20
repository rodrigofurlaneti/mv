import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { PeopleProvider } from '../../../providers/people/people';
import { AuthProvider } from '../../../providers/auth/auth';
import { UtilsProvider } from '../../../providers/utils/utils';

/**
 * Generated class for the ClientAddressPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-client-address',
  templateUrl: 'client-address.html',
})
export class ClientAddressPage {

  enderecos: any = [];
  isLoaded: boolean = false;

  constructor(public navCtrl: NavController,
    private peopleProvider: PeopleProvider,
    private authProvider: AuthProvider,
    private utils: UtilsProvider,
    public navParams: NavParams) {
  }

  ionViewDidEnter() {
    this.authProvider.getUserLogged()
      .subscribe(data => {
        this.peopleProvider.getPessoa(data.PessoaId)
          .subscribe(
            data => {
              this.enderecos = data.EnderecosEntrega || [];
              this.isLoaded = true;
            },
            error => {
              this.utils.showToast(error);
              this.isLoaded = true;
            }
          )
      }, error => {
        this.isLoaded = true;
        this.utils.showToast(error);
      });
  }

  editar(endereco) {
    this.navCtrl.push('ClientAddressEditPage', {"endereco": endereco});
  }

  add() {
    this.navCtrl.push('ClientAddressEditPage');
  }

  remover(endereco) {
      this.peopleProvider.removeAddress(endereco).subscribe(
        data => {
          this.utils.showToast('Endereço removido com sucesso.');
          var index = this.enderecos.indexOf(endereco);
          this.enderecos.splice(index, 1);
      }, err => {
          this.utils.showToast(err.text());
      });
  }

}
