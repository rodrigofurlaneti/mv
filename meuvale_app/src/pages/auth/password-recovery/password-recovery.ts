import { Component } from '@angular/core';
import { IonicPage, ViewController, NavParams } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { AuthProvider } from '../../../providers/auth/auth';

@IonicPage()
@Component({
  selector: 'page-password-recovery',
  templateUrl: 'password-recovery.html',
})
export class PasswordRecoveryPage {

  recoveryForm: FormGroup = new FormGroup({
    userName: new FormControl('', [Validators.required])
  });

  constructor(public navParams: NavParams,
    private utils: UtilsProvider,
    private authProvider: AuthProvider,
    private viewCtrl: ViewController) { }

  ionViewDidEnter() {
    const _username = this.navParams.get("username");
    if (_username)
      this.recoveryForm.value.userName = _username;
  }

  doRecovery(): void {
    if (this.recoveryForm.valid) {
      this.authProvider.rememberPassword(this.recoveryForm.value.userName)
        .subscribe(
          (data) => {
            this.utils.showToast("Uma nova senha foi enviada no e-mail de cadastro!");
          },
          (error) => {
            this.utils.showToast("Erro! Contate a central de atendimento");
          }
        );
    } else {
      this.utils.showToast("preencha os campos corretamente!");
    }
  }

  dismiss() {
    this.viewCtrl.dismiss();
  }
}
