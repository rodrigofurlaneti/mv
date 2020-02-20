import { Component } from '@angular/core';
import { IonicPage, NavParams, NavController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';
import { GeolocationProvider } from '../../../providers/google/geolocation';
import { CepProvider } from '../../../providers/adress/cep';
import { ConvenioProvider } from '../../../providers/convenio/convenio';

@IonicPage()
@Component({
  selector: 'page-convenio-register',
  templateUrl: 'convenio-register.html'
})
export class ConvenioRegisterPage {

  storeForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      cnpj: new FormControl('', [Validators.required]),
      nomefantasia: new FormControl('', [Validators.required]),
      razaosocial: new FormControl(''),
      cep: new FormControl('', [Validators.required]),
      logradouro: new FormControl('', [Validators.required]),
      numero: new FormControl('', [Validators.required]),
      complemento: new FormControl(),
      paisSelected: new FormControl('', [Validators.required]),
      estadoSelected: new FormControl('', [Validators.required]),
      cidadeSelected: new FormControl('', [Validators.required]),
      ativo: new FormControl(true),
      pessoaFisica: new FormControl(false)
    }
  );

  paises: any[] = [];
  estados: any[] = [];
  cidades: any[] = [];
  convenio: any;
  latitude: any;
  logintude: any;

  constructor(
    private utils: UtilsProvider,
    private peopleProvider: PeopleProvider,
    private navParams: NavParams,
    private navCtrl: NavController,
    private convenioProvider: ConvenioProvider,
    private geolocationProvider: GeolocationProvider,
    private cepProvider: CepProvider
  ) {

    this.convenio = this.navParams.get("loja");

    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para ver os Convênios.');
      this.navCtrl.setRoot("LoginPage");
      return;
    }
  }

  ionViewDidLoad() {
    if (HttpProvider.userAuth) {
      if (this.convenio) {
        this.utils.showLoader("Carregando o convênio");
        this.convenioProvider.getById(this.convenio.Id)
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
                if (this.convenio) {
                  this.peopleProvider.getCidades(this.convenio.Endereco.Cidade.Estado.Id)
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

  onChangeCep() {
    if (this.storeForm.value.cep.length < 9) {
      this.utils.showToast("CEP inválido");
      return;
    }

    this.cepProvider.getAdressByCep(this.storeForm.value.cep.replace('-', ''))
      .subscribe(
        data => {

          if (!data || !data.logradouro) {
            this.utils.showToast("CEP não encontrado");
            return;
          }

          this.storeForm.get('logradouro').setValue(data.logradouro);

          this.peopleProvider.getCidades(this.estados.find(y => y.Sigla == data.uf).Id)
            .subscribe(
              cidades => {
                this.cidades = cidades || [];

                this.storeForm.get('paisSelected').setValue(this.cidades.find(x => x.Descricao == data.localidade).Estado.Pais.Id);
                this.storeForm.get('cidadeSelected').setValue(this.cidades.find(x => x.Descricao == data.localidade).Id);
                this.storeForm.get('estadoSelected').setValue(this.cidades.find(x => x.Descricao == data.localidade).Estado.Id);
              },
              error => this.utils.showToast(error)
            )
        })
  }

  prepareForm(data: any): void {
    if (data) {
      this.storeForm.setValue(
        {
          id: data.Id,
          cnpj: data.Cnpj,
          nomefantasia: data.Descricao,
          razaosocial: data.RazaoSocial,
          cep: data.Endereco ? data.Endereco.Cep : '',
          logradouro: data.Endereco ? data.Endereco.Logradouro : '',
          numero: data.Endereco ? data.Endereco.Numero : '',
          complemento: data.Endereco ? data.Endereco.Complemento : '',
          paisSelected: data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Estado ?
            data.Endereco.Cidade.Estado.Pais ? data.Endereco.Cidade.Estado.Pais.Id : '' : '' : '' : '',
          estadoSelected: data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Estado ?
            data.Endereco.Cidade.Estado.Id : '' : '' : '',
          cidadeSelected: data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Id : '' : '',
          ativo: data.Status,
          pessoaFisica: data.Cnpj.length <= 14
        }
      )
    }
    this.utils.hideLoader();
  }

  dismiss() {
    this.navCtrl.pop();
  }

  doSubmit() {
    if (this.storeForm.valid) {
      this.utils.showLoader("Registrando convênio");

      let _data = null;
      let logradouro = this.storeForm.value.logradouro ? this.storeForm.value.logradouro : '';
      let numero = this.storeForm.value.numero ? this.storeForm.value.numero : '';

      this.geolocationProvider.getGeolocationByAdress(logradouro + "," + numero)
        .subscribe(
          retorno => {
            if (retorno.results.length > 0) {
              this.latitude = retorno.results[0].geometry ? retorno.results[0].geometry.location.lat : null;
              this.logintude = retorno.results[0].geometry ? retorno.results[0].geometry.location.lng : null;
            }

            _data = this.formToUserData();

            if (HttpProvider.userAuth) {
              this.convenioProvider.save(_data)
                .subscribe(() => {
                  this.utils.hideLoader();
                  this.utils.showAlert("Sucesso", "Convênio atualizado com sucesso!");
                }, error => {
                  this.utils.hideLoader();
                  this.utils.showToast(error.error);
                });
            }
            else {
              this.utils.showAlert("Erro", "Usuário não autenticado!");
            }

          }
        )
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
      RazaoSocial: _formData.razaosocial,
      Cnpj: _formData.cnpj,
      Endereco: {
        Tipo: 1,
        Cep: _formData.cep,
        Logradouro: _formData.logradouro,
        Numero: _formData.numero,
        Complemento: _formData.complemento,
        Bairro: _formData.bairro,
        Cidade: {
          Id: _formData.cidadeSelected,
          Estado: {
            Id: _formData.estadoSelected,
            Pais: {
              Id: _formData.paisSelected
            }
          }
        },
        Latitude: this.latitude,
        Longitude: this.logintude
      },
      Status: _formData.ativo,
    };

    return data;
  }

  getLatitude(adress) {
    this.geolocationProvider.getGeolocationByAdress(adress)
      .subscribe(
        data => {
          this.latitude = data.results[0].geometry.location.lat
          this.logintude = data.results[0].geometry.location.lng
        }
      )
  }
}
