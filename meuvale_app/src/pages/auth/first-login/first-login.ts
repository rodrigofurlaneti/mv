import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { PeopleProvider } from '../../../providers/people/people';
import { MyApp } from '../../../app/app.component';

@IonicPage()
@Component({
  selector: 'page-first-login',
  templateUrl: 'first-login.html',
})
export class FirstLoginPage {

  clientForm: FormGroup = new FormGroup(
    {
      senha: new FormControl('', [Validators.required]),
      confirmaSenha: new FormControl(''),
      celular: new FormControl(''),
      email: new FormControl('', [Validators.required, Validators.email])
    }
  );
  perfilId: any;
  static convenio: string;
  static combo: string;

  constructor(public navCtrl: NavController,
    private navParams: NavParams,
    private peopleProvider: PeopleProvider,
    private myApp: MyApp,
    private utils: UtilsProvider) {
    this.perfilId = this.navParams.get("perfil");
    FirstLoginPage.convenio = this.navParams.get("convenio");
    FirstLoginPage.combo = this.navParams.get("combo");
  }

  doSubmit() {
    if ((this.clientForm.valid && (HttpProvider.userAuth || this.clientForm.value.senha != ''))) {
      if (this.clientForm.value.senha == this.clientForm.value.confirmaSenha) {
        this.utils.showLoader("Registrando dados!");
        const _data = this.formToUserData();
        this.peopleProvider.alterpassword(_data)
          .subscribe(data => {
            this.utils.hideLoader();
            this.utils.showAlert("Sucesso", "Dados atualizados com sucesso!");
            _data.PerfilId = this.perfilId;
            _data.PrimeiroLogin = "FALSE";
            HttpProvider.userAuth.PrimeiroLogin = "FALSE";
            MyApp.exibeTutorialQrCode = true;

            if (!FirstLoginPage.combo && FirstLoginPage.combo === "" && FirstLoginPage.convenio && FirstLoginPage.convenio != "")
              MyApp.redirect = "ProductPlanOrderPage";
            else
              MyApp.redirect = "TutorialPage";

            this.myApp.handleAuthChange(_data);
          }, error => {
            this.utils.hideLoader();
            this.utils.showToast(error.error);
          });

      } else {
        this.utils.showToast("Senhas não conferem!");
      }
    } else {
      this.utils.showToast("Preencha os campos corretamente!");
    }
  }

  private formToUserData(): any {
    const _formData = this.clientForm.value;
    const _pessoa: any = { Id: HttpProvider.userAuth.PessoaId }

    let data = {
      Id: HttpProvider.userAuth.UsuarioId,
      Pessoa: {
        Id: HttpProvider.userAuth.PessoaId,
        Contatos: [
          {
            Contato: {
              Email: _formData.email,
              Tipo: 1,
              Id: 0,
              Pessoa: _pessoa

            }
          },
          {
            Contato: {
              Numero: _formData.celular,
              Tipo: 3,
              Id: 0,
              Pessoa: _pessoa
            }
          }
        ]
      },
      senha: _formData.senha,
      PrimeiroLogin: false
    };
    return data;
  }

}
