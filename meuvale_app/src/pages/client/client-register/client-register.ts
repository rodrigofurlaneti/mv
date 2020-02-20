import { Component } from '@angular/core';
import { IonicPage, ViewController, NavParams, NavController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';
import { Observable } from 'rxjs';
import { AuthProvider } from '../../../providers/auth/auth';
import { PushSetupProvider } from '../../../providers/push-setup/push-setup';

@IonicPage()
@Component({
  selector: 'page-client-register',
  templateUrl: 'client-register.html'
})
export class ClientRegisterPage {

  isFacebookRegister: boolean = false;
  facebookId: string;
  sended: boolean = false;

  clientForm: FormGroup = new FormGroup(
    {
      cpf: new FormControl('', [Validators.required]),
      nome: new FormControl('', [Validators.required]),
      sexo: new FormControl(''),
      dataNascimento: new FormControl(''),
      celular: new FormControl(''),
      email: new FormControl('', [Validators.required, Validators.email]),
      senha: new FormControl('', [Validators.required]),
      confirmaSenha: new FormControl('')
    }
  );

  constructor(public navCtrl: NavController,
    private viewCtrl: ViewController,
    private utils: UtilsProvider,
    private authProvider: AuthProvider,
    private navParams: NavParams,
    private pushProvider: PushSetupProvider,
    private peopleProvider: PeopleProvider
  ) { }

  ionViewDidEnter() {
    if (HttpProvider.userAuth) {
      this.utils.showLoader("Carregando usuário");
      this.peopleProvider.getPessoa(HttpProvider.userAuth.PessoaId)
        .subscribe(
          data => this.prepareForm(data),
          error => {
            this.utils.hideLoader();
            this.utils.showToast(error.error);
          }
        )
    } else if (this.navParams.get('facebook')) {
      this.isFacebookRegister = true;
      this.facebookId = this.navParams.get("facebookId");
      this.clientForm.controls['email'].setValue(this.navParams.get('email'));
    }
  }

  prepareForm(data: any): void {
    this.utils.hideLoader();
    this.clientForm.setValue(
      {
        cpf: data.Cpf,
        nome: data.Nome,
        sexo: data.Sexo,
        dataNascimento: this.utils.formatData(data.DataNascimento),
        celular: data.Contatos && data.Contatos.length && data.Contatos.length > 1 && data.Contatos[1].Contato && data.Contatos[1].Contato.Numero ? data.Contatos[1].Contato.Numero : '',
        email: data.Email,
        senha: '',
        confirmaSenha: ''
      }
    )
  }

  dismiss() {
    this.viewCtrl.dismiss();
  }

  doSubmit() {
    this.sended = true;
    if ((this.clientForm.valid && (HttpProvider.userAuth || this.clientForm.value.senha != '')) || this.isFacebookRegister) {
      if (this.clientForm.value.senha == this.clientForm.value.confirmaSenha || this.isFacebookRegister) {
        this.utils.showLoader("Registrando dados!");
        const _data = this.formToUserData();

        if (HttpProvider.userAuth) {
          this.peopleProvider.save(_data)
            .subscribe(data => {
              this.utils.hideLoader();
              this.utils.showAlert("Sucesso", "Dados atualizados com sucesso!");
              this.viewCtrl.dismiss();
            }, error => {
              this.utils.hideLoader();
              this.utils.showToast(error.error);
            });
        } else {
          this.peopleProvider.save(_data)
            .subscribe(data => {
              this.procedLogin();
            }, error => {
              this.utils.hideLoader();
              this.utils.showToast(error.error);
            });
        }

      } else {
        this.utils.showToast("Senhas não conferem!");
      }

    } else {
      this.utils.showToast("Preencha os campos corretamente!");
    }
  }

  private procedLogin(): any {
    let service: Observable<any>;
    if (this.isFacebookRegister) {
      service = this.authProvider.authenticate({ name: this.clientForm.value.email, password: this.facebookId });
    } else {
      service = this.authProvider.authenticate({ name: this.clientForm.value.email, password: this.clientForm.value.senha });
    }

    service.subscribe(
      data => {
        this.pushProvider.pushsetup(HttpProvider.userAuth.UsuarioId)
        this.utils.hideLoader();
        this.dismiss();
      }, error => {
        this.utils.hideLoader();
        this.utils.showToast(error.error);
      }
    )
  }

  get logado(): boolean {
    return !!HttpProvider.userAuth;
  }

  private formToUserData(): any {

    const _formData = this.clientForm.value;

    const _pessoa: any = {
      Nome: _formData.nome,
      DataNascimento: this.utils.dataToFromBody(_formData.dataNascimento),
      Sexo: _formData.sexo,
    }

    let data: any = {
      Pessoa: {
        Nome: _formData.nome,
        DataNascimento: this.utils.dataToFromBody(_formData.dataNascimento),
        Sexo: _formData.sexo,
        Documentos: [
          {
            Numero: _formData.cpf,
            Tipo: 2,
            Id: 0,
            Pessoa: _pessoa
          }
        ],
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
      }
    }
    if (HttpProvider.userAuth) {
      data.Id = HttpProvider.userAuth.UsuarioId;
      data.Pessoa.Id = HttpProvider.userAuth.PessoaId;
    }
    data.senha = _formData.senha;

    if (this.isFacebookRegister) {
      data.FacebookId = this.facebookId;
    }
    return data;
  }

}
