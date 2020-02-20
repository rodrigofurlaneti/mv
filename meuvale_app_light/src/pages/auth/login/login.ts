import { Component } from '@angular/core';
import { IonicPage, NavController, ModalController, NavParams, Platform } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthProvider } from '../../../providers/auth/auth';
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

    if (MyApp.modalRedirectLogin && this.redirect) {
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

}
