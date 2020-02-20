import { Component } from '@angular/core';
import { IonicPage, NavController, ModalController, NavParams, Platform } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthProvider } from '../../../providers/auth/auth';
import { FacebookProvider } from '../../../providers/facebook/facebook';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PushSetupProvider } from '../../../providers/push-setup/push-setup';
import { HttpProvider } from '../../../providers/http/http';
import { MyApp } from '../../../app/app.component';

@IonicPage()
@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
})
export class LoginPage {

  loginForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });
  redirect: string = "";
  loja: any;

  constructor(public navCtrl: NavController,
    private authProvider: AuthProvider,
    private facebookProvider: FacebookProvider,
    private pushProvider: PushSetupProvider,
    private mdlCtrl: ModalController,
    private navParams: NavParams,
    private platform: Platform,
    private utils: UtilsProvider) {

    this.redirect = this.navParams.get("redirect");
    MyApp.tabRedirect = this.navParams.get("tabRedirect");
    this.loja = this.navParams.get("loja");
  }

  doRecovery(): void {
    this.mdlCtrl.create("PasswordRecoveryPage", { username: this.loginForm.value.name }).present();
  }

  doRegistry(): void {
    let mdl = this.mdlCtrl.create("ClientRegisterSimplePage");
    mdl.onDidDismiss(
      data => {
        if (MyApp.modalRedirectLogin)
          this.navCtrl.pop();
      }
    );
    mdl.present();
  }

  doLoginFacebook(): void {
    this.utils.showLoader("connectando ao facebook");
    console.log("connectando ao facebook");
    this.facebookProvider.facebookLogin()
      .subscribe(
        data => {
          console.log(data);
          console.log("facebook provider ok...");
          this.authProvider.authenticateFacebook(data)
            .subscribe(
              data => {
                console.log("logado: ");
                console.log(data);
                if (data) {
                  this.procedLogin();
                } else {
                  this.utils.showAlert("Erro", "Não foi possível logar com o facebook");
                }
              },
              error => {
                if (error.status == 401) {
                  console.log("Retorno facebook: ")
                  console.log(data);
                  this.navCtrl.push("ClientRegisterPage", {
                    facebook: true,
                    facebookId: data.facebookId,
                    email: data.email
                  })
                } else {
                  this.handleError("Login ou senha inválidos, tente novamente por favor!");
                }
              },
              () => this.utils.hideLoader()
            )
        },
        error => this.handleError(error),
        () => this.utils.hideLoader()
      );
  }

  doLogin(): void {
    if (this.loginForm.valid) {
      this.utils.showLoader("efetuando login");
      this.authProvider.authenticate(this.loginForm.value)
        .subscribe(
          data => this.procedLogin(),
          error => this.handleError("Login ou senha inválidos, tente novamente por favor!"),
          () => this.utils.hideLoader()
        );
    } else {
      this.utils.showToast("preencha os dados corretamente...");
    }
  }

  private procedLogin(): void {
    if (this.platform.is('ios') || this.platform.is('android'))
      this.pushProvider.pushsetup(HttpProvider.userAuth.UsuarioId)

    if (MyApp.modalRedirectLogin && this.redirect && this.redirect == 'MainMeuValePage') {
      this.navCtrl.pop();
      MyApp.redirect = this.redirect;
      MyApp.lojaRedirect = this.loja;
    }
    
    if (MyApp.modalRedirectLogin) {
      this.navCtrl.pop();
    }
    
    if (this.redirect) {
      MyApp.redirect = this.redirect;
      MyApp.lojaRedirect = this.loja;
    }
  }

  private handleError(error: any): void {
    this.utils.hideLoader();
    this.utils.showToast(error);
  }

  tutorial(){
    this.navCtrl.push("TutorialPage");
  }

}
