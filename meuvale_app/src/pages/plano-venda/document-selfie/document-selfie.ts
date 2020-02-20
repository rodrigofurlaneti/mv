import { Component, ViewChild, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams, ModalController } from 'ionic-angular';
import { Slides } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpProvider } from '../../../providers/http/http';

@IonicPage()
@Component({
  selector: 'page-document-selfie',
  templateUrl: 'document-selfie.html',
})
export class DocumentSelfiePage implements OnInit {
  constructor(public navCtrl: NavController,
    public navParams: NavParams) {
  }

  ngOnInit(): void {
    this.pessoa.Id = HttpProvider.userAuth.PessoaId;
  }

  @ViewChild(Slides) slides: Slides;

  capturar: string = '';
  pessoa = {Id: ""};
  data = new Date().getDate().toString() + new Date().getMonth().toString() + new Date().getFullYear().toString()
  + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString();

  fotoForm: FormGroup = new FormGroup(
    {
      selfie: new FormControl('', [Validators.required]),
    }
  );


handleEvent(e) {
  this.fotoForm.value.foto = e.URL;
  this.capturar = e.URL ;
  }
  
  //não mexer nessa classe
  prosseguir(){
    this.navCtrl.push('ProductPlanOrderPage')
  }
}
