import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';

/**
 * Generated class for the ContactPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-contact',
  templateUrl: 'contact.html',
})
export class ContactPage {

  contactForm: FormGroup = new FormGroup({
    Usuario: new FormControl('', [Validators.required]),
    Email: new FormControl('', [Validators.required, Validators.email]),
    Telefone: new FormControl('', [Validators.required]),
    Assunto: new FormControl('', [Validators.required]),
    Descricao: new FormControl('', [Validators.required]),
  });
  tipoContato: any;

  constructor(public navCtrl: NavController,
    private peopleProvider: PeopleProvider,
    private utils: UtilsProvider,
    public navParams: NavParams) {
    this.tipoContato = this.navParams.get("tipo");
    if (this.tipoContato) {
      this.contactForm.setValue({
        Assunto: this.tipoContato,
        Descricao: "Cliente possuí interesse em: " + this.tipoContato,
        Usuario: HttpProvider.userAuth ? HttpProvider.userAuth.UsuarioNome : "",
        Email: "",
        Telefone: ""
      })
    };
  }

  ionViewDidLoad() {

  }

  doContact() {
    if (this.contactForm.valid) {
      this.utils.showLoader("salvando dados!");
      this.peopleProvider.saveContact(this.contactForm.value)
        .subscribe(
          data => {
            this.utils.hideLoader();
            this.contactForm.reset();
            this.utils.showToast("Contato salvo com sucesso!");
          }
        )
    } else {
      this.utils.showToast("Preencha todos os campos corretamente")
    }
  }

}
