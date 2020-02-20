import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthProvider } from '../../../providers/auth/auth';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PushSetupProvider } from '../../../providers/push-setup/push-setup';
import { HttpProvider } from '../../../providers/http/http';
import { PeopleProvider } from '../../../providers/people/people';
import { Observable } from 'rxjs';
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
      confirmaSenha: new FormControl('')
    }
  );
  perfilId: any;

  constructor(public navCtrl: NavController,
    private navParams: NavParams,
    private peopleProvider: PeopleProvider,
    private myApp: MyApp,
    private utils: UtilsProvider) {
      this.perfilId = this.navParams.get("perfil");
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
    let data = {
      Id: HttpProvider.userAuth.UsuarioId,
      Pessoa: { Id: HttpProvider.userAuth.PessoaId },
      senha: _formData.senha,
      PrimeiroLogin: false
    };
    return data;
  }

}
