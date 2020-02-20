import { Component } from '@angular/core';
import { IonicPage, NavParams, NavController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoreProvider } from '../../../providers/store/store';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';
import { GeolocationProvider } from '../../../providers/google/geolocation';
import { CepProvider } from '../../../providers/adress/cep';
import { PartnerProvider } from '../../../providers/partner/partner';

@IonicPage()
@Component({
  selector: 'page-store-register',
  templateUrl: 'store-register.html'
})
export class StoreRegisterPage {

  storeForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      cnpj: new FormControl('', [Validators.required]),
      ie: new FormControl(''),
      nomefantasia: new FormControl('', [Validators.required]),
      razaosocial: new FormControl('', [Validators.required]),
      lojafisica: new FormControl('', [Validators.required]),
      telefone: new FormControl('', [Validators.required]),
      celular: new FormControl(''),
      email: new FormControl('', [Validators.required, Validators.email]),
      cep: new FormControl('', [Validators.required]),
      logradouro: new FormControl('', [Validators.required]),
      numero: new FormControl('', [Validators.required]),
      complemento: new FormControl(),
      paisSelected: new FormControl('', [Validators.required]),
      estadoSelected: new FormControl('', [Validators.required]),
      cidadeSelected: new FormControl('', [Validators.required]),
      ativo: new FormControl(''),
      classificacao: new FormControl(''),
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
    private partnerProvider: PartnerProvider,
    private geolocationProvider: GeolocationProvider,
    private cepProvider: CepProvider
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

  handleEvent(e) {
    this.loja.Logo = e.URL;
    this.loja.LogoUpload = e.URL;
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
          ie: data.InscricaoEstadual,
          nomefantasia: data.Descricao,
          razaosocial: data.RazaoSocial,
          telefone: data.Telefone,
          celular: data.Celular,
          email: data.Email,
          lojafisica: data.Endereco ? 'Sim' : 'Nao',
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
          classificacao: data.Classificacao
        }
      )
      this.loja.Logo = data.Logo;
      this.lojaAprovada = data.LojaAprovada;
    }
    this.utils.hideLoader();
  }

  dismiss() {
    if (this.fornecedor && this.fornecedor == true)
      this.navCtrl.setRoot("PartnerStoreListPage");
    else
      this.navCtrl.setRoot("OursStoreListPage");
  }

  doSubmit() {
    if (this.storeForm.valid) {
      this.utils.showLoader("Registrando estabelecimento");

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
              if (!this.fornecedor || this.fornecedor == false)
                this.storeProvider.salvarLoja(_data)
                  .subscribe(data => {
                    this.utils.hideLoader();
                    this.utils.showAlert("Sucesso", "Estabelecimento atualizado com sucesso!");
                  }, error => {
                    this.utils.hideLoader();
                    this.utils.showToast(error.error);
                  });
              else
                this.partnerProvider.salvarfornecedor(_data)
                  .subscribe(data => {
                    this.utils.hideLoader();
                    this.utils.showAlert("Sucesso", "Estabelecimento atualizado com sucesso!");
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
      Telefone: _formData.telefone,
      Cnpj: _formData.cnpj,
      InscricaoEstadual: _formData.ie,
      Celular: _formData.celular,
      Email: _formData.email,
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
      LojaAprovada: this.lojaAprovada,
      LogoUpload: this.loja.Logo,
      Classificacao: _formData.classificacao
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

  openSchedule() {
    this.navCtrl.push("StoreSchedulePage", { loja: this.loja })
  }

  openDelivery() {
    this.navCtrl.push("StoreDeliveryPage", { loja: this.loja });
  }
}
