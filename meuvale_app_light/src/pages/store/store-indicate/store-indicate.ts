import { Component } from '@angular/core';
import { IonicPage, NavParams, NavController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoreProvider } from '../../../providers/store/store';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';
import { PartnerProvider } from '../../../providers/partner/partner';

@IonicPage()
@Component({
  selector: 'page-store-indicate',
  templateUrl: 'store-indicate.html'
})
export class StoreIndicatePage {

  storeForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      cnpj: new FormControl(''),
      nomefantasia: new FormControl('', [Validators.required]),
      telefone: new FormControl('', [Validators.required]),
      celular: new FormControl(''),
      paisSelected: new FormControl('', [Validators.required]),
      estadoSelected: new FormControl('', [Validators.required]),
      cidadeSelected: new FormControl('', [Validators.required])
    }
  );
  paises: any[] = [];
  estados: any[] = [];
  cidades: any[] = [];
  loja: any;
  latitude: any;
  logintude: any;
  lojaAprovada: false;
  fornecedor: Boolean = false;
  date = new Date().getDate().toString() + new Date().getHours().toString();

  constructor(
    private utils: UtilsProvider,
    private peopleProvider: PeopleProvider,
    private navParams: NavParams,
    private navCtrl: NavController,
    private storeProvider: StoreProvider,
    private partnerProvider: PartnerProvider
  ) {

    this.loja = this.navParams.get("loja");
    this.fornecedor = this.navParams.get("fornecedor");

    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para ver as suas lojas.');
      this.navCtrl.setRoot("LoginPage");
      return;
    }
  }

  ionViewDidLoad() {
    if (HttpProvider.userAuth) {
      if (this.loja) {
        this.utils.showLoader("Carregando seu estabelecimento");
        if (!this.fornecedor)
          this.storeProvider.getLoja(this.loja.Id)
            .subscribe(
              data => this.prepareForm(data),
              error => {
                this.utils.hideLoader();
                this.utils.showToast(error.error);
              }
            )
        else
          this.partnerProvider.getfornecedor(this.loja.Id)
            .subscribe(
              data => this.prepareForm(data),
              error => {
                this.utils.hideLoader();
                this.utils.showToast(error.error);
              }
            )
      }
    }

    this.loadData();
  }

  loadData() {
    this.peopleProvider.getPaises()
      .subscribe(
        data => {
          this.paises = data || [];
          if (this.paises.length) {
            this.storeForm.controls['paisSelected']
              .setValue(this.paises[0].Id)
          }

          this.peopleProvider.getEstados()
            .subscribe(
              data => {
                this.estados = data || [];
                if (this.loja) {
                  this.peopleProvider.getCidades(this.loja.Endereco.Cidade.Estado.Id)
                    .subscribe(
                      data => {
                        this.cidades = data || [];
                      },
                      error => this.utils.showToast(error)
                    )
                }
              },
              error => this.utils.showToast(error)
            );
        },
        error => this.utils.showToast(error)
      );
  }

  onChangeEstado() {
    console.log("cidades")
    this.peopleProvider.getCidades(this.storeForm.value.estadoSelected)
      .subscribe(
        data => this.cidades = data || [],
        error => this.utils.showToast(error)
      )
  }

  prepareForm(data: any): void {
    if (data) {
      this.storeForm.setValue(
        {
          id: data.Id,
          cnpj: data.Cnpj,
          nomefantasia: data.Descricao,
          telefone: data.Telefone,
          celular: data.Celular,
          paisSelected: data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Estado ?
            data.Endereco.Cidade.Estado.Pais ? data.Endereco.Cidade.Estado.Pais.Id : '' : '' : '' : '',
          estadoSelected: data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Estado ?
            data.Endereco.Cidade.Estado.Id : '' : '' : '',
          cidadeSelected: data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Id : '' : ''
        }
      )
    }
    this.utils.hideLoader();
  }

  doSubmit() {
    if (this.storeForm.valid) {
      this.utils.showLoader("Registrando indicação");

      let _data = null;
      _data = this.formToUserData();

      if (HttpProvider.userAuth) {
        this.storeProvider.salvarLojaIndicacao(_data)
          .subscribe(data => {
            this.utils.hideLoader();
            this.utils.showAlert("Sucesso", "Indicação registrada com sucesso! Obrigado.");
            this.navCtrl.pop();
          }, error => {
            this.utils.hideLoader();
            this.utils.showToast(error.error);
          });
      }
      else {
        this.utils.showAlert("Erro", "Usuário não autenticado!");
      }
    }
    else {
      this.utils.showToast("Preencha os campos corretamente!");
    }
  }

  private formToUserData(): any {

    const _formData = this.storeForm.value;
    let data: any

    data = {
      Id: _formData.id,
      Descricao: _formData.nomefantasia,
      Telefone: _formData.telefone,
      Cnpj: _formData.cnpj,
      Celular: _formData.celular,
      Endereco: {
        Tipo: 1,
        Cidade: {
          Id: _formData.cidadeSelected,
          Estado: {
            Id: _formData.estadoSelected,
            Pais: {
              Id: _formData.paisSelected
            }
          }
        },
      }
    };

    return data;
  }
}
