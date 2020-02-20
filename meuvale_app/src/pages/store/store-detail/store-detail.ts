import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LaunchNavigator } from '@ionic-native/launch-navigator';
@IonicPage()
@Component({
  selector: 'page-store-detail',
  templateUrl: 'store-detail.html',
})
export class StoreDetailPage {
  
  loja: any = {};
  endereco: any = {};

  constructor(public navCtrl: NavController, 
    private launchNavigator: LaunchNavigator,
    public navParams: NavParams) {
      this.loja = this.navParams.get("loja");
      console.log(this.loja)
      this.maper(this.loja);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad StoreDetailPage');
  }

  private maper(loja) {
        
    this.endereco = {
        //Id: loja.Endereco.Id,
        Tipo: loja.Endereco.Tipo,
        Cep: loja.Endereco.Cep,
        Logradouro: loja.Endereco.Logradouro,
        Numero: loja.Endereco.Numero,
        Complemento: loja.Endereco.Complemento,
        Bairro: loja.Endereco.Bairro,
        Cidade: loja.Endereco.Cidade ? loja.Endereco.Cidade.Descricao: "",
        CidadeId: loja.Endereco.Cidade ? loja.Endereco.Cidade.Id : 4181,
        Estado: loja.Endereco.Cidade ? loja.Endereco.Cidade.Estado.Sigla : "",
        EstadoId: loja.Endereco.Cidade ? loja.Endereco.Cidade.Estado.Id : 26
    };
  }

  navegarpara()
  {
      var navegarpara = this.endereco.Logradouro + "," + this.endereco.Numero;

      this.launchNavigator.navigate(navegarpara)
      .then(
          () => console.log('Launched navigator'),
          error => console.log('Error launching navigator', error)
      );
  }

}
