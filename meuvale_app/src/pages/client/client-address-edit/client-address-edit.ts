import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, ViewController } from 'ionic-angular';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';
import { Observable } from 'rxjs';
import { CepProvider } from '../../../providers/adress/cep';
import { GeolocationProvider } from '../../../providers/google/geolocation';
import { Geolocation } from '@ionic-native/geolocation';

@IonicPage()
@Component({
  selector: 'page-client-address-edit',
  templateUrl: 'client-address-edit.html',
})
export class ClientAddressEditPage {

  paises: any[] = [];
  estados: any[] = [];
  cidades: any[] = [];
  ender: any;
  denovo: any;
  modalMode: any;
  enderecoId: any;
  addressForm: FormGroup = new FormGroup(
    {
      descricao: new FormControl('', [Validators.required]),
      cep: new FormControl('', [Validators.required]),
      logradouro: new FormControl('', [Validators.required]),
      numero: new FormControl('', [Validators.required]),
      complemento: new FormControl(),
      bairro: new FormControl(),
      paisSelected: new FormControl('', [Validators.required]),
      estadoSelected: new FormControl('', [Validators.required]),
      cidadeSelected: new FormControl('', [Validators.required]),
    }
  )
  
  constructor(public navCtrl: NavController, 
    public navParams: NavParams,
    private peopleProvider: PeopleProvider,
    private viewCtrl: ViewController,
    private cepProvider: CepProvider,
    private geolocationProvider: GeolocationProvider,
    private geo: Geolocation,
    private utils: UtilsProvider) {
      this.ender = navParams.get("endereco");
      this.modalMode = this.navParams.get("modalMode");
      this.denovo = 1;
  }

  dismiss() {
    this.viewCtrl.dismiss();
  }

  ionViewDidEnter() {
    console.log('ionViewDidLoad ClientAddressEditPage');
    this.loadData();
  }

  loadData(){
    this.peopleProvider.getPaises()
      .subscribe(
        data => {
          this.paises = data || [];
          if(this.paises.length) {
            this.addressForm.controls['paisSelected']
              .setValue(this.paises[0].Id)
          }

          this.peopleProvider.getEstados()
            .subscribe(
              data => {
                this.estados = data || [];
                var estadoFind = this.estados.find(x => x.Descricao == "São Paulo").Id;
                this.addressForm.value.estadoSelected = estadoFind;
                
                if (this.ender) {
                  this.enderecoId = this.ender.Id;
                  this.peopleProvider.getCidades(this.ender.Cidade.Estado.Id)
                    .subscribe(
                      data => { 
                        this.cidades = data || [];
                        this.prepareForm(this.ender);
                      },
                      error => this.utils.showToast(error)
                    )
                }
                else
                {  
                  this.peopleProvider.getCidades(estadoFind)
                    .subscribe(
                      data => { 
                        this.cidades = data || [];
                        this.addressForm.controls['estadoSelected'].setValue(estadoFind);
                        this.addressForm.controls['cidadeSelected'].setValue(this.cidades.find(x=> x.Descricao == "São Paulo").Id);
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

  prepareForm(ender: any): any {
    //console.log(ender);
    if (ender) {
      this.addressForm.setValue(
        {
          descricao: ender.Descricao,
          cep: ender.Cep,
          logradouro: ender.Logradouro,
          numero: ender.Numero,
          complemento: ender.Complemento,
          bairro: ender.Bairro,
          paisSelected: this.paises[0].Id,
          estadoSelected: ender.Cidade.Estado.Id,
          cidadeSelected: ender.Cidade.Id
        }
      );
    }
  }

  onChangeEstado() {
    console.log("cidades");
    this.peopleProvider.getCidades(this.addressForm.value.estadoSelected)
      .subscribe(
        data => {
          this.cidades = data || [];
        },
        error => this.utils.showToast(error)
      )
  }

  onChangeCep() {
    if (this.addressForm.value.cep.length < 9) {
      this.utils.showToast("CEP inválido");
      return;
    }

    this.cepProvider.getAdressByCep(this.addressForm.value.cep.replace('-', ''))
      .subscribe(
        data => {

          if (!data || !data.logradouro) {
            this.utils.showToast("CEP não encontrado");
            return;
          }
          
          this.addressForm.get('logradouro').setValue(data.logradouro);
          this.addressForm.get('bairro').setValue(data.bairro);

          this.peopleProvider.getCidades(this.estados.find(y => y.Sigla == data.uf).Id)
            .subscribe(
              cidades => {
                this.cidades = cidades || [];

                this.addressForm.get('paisSelected').setValue(this.cidades.find(x => x.Descricao == data.localidade).Estado.Pais.Id);
                this.addressForm.get('cidadeSelected').setValue(this.cidades.find(x => x.Descricao == data.localidade).Id);
                this.addressForm.get('estadoSelected').setValue(this.cidades.find(x => x.Descricao == data.localidade).Estado.Id);
              },
              error => this.utils.showToast(error)
            )
        })
  }

  doSubmit() {
    if(this.addressForm.valid) {
      this.utils.showLoader("aguarde");
      const _address = this.getValidAddress();
      let service : Observable<any>;
      if (!this.enderecoId) {
        service = this.peopleProvider.saveAddress(_address);
      } else {
        service = this.peopleProvider.updateAddress(_address);
      }

      service.subscribe(
        data => {
          this.utils.hideLoader();
          this.utils.showAlert("Sucesso",
            this.enderecoId ? "Endereço alterado com sucesso" : "Endereço salvo com sucesso"
          );
          this.navCtrl.pop();
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error.error);
        }
      )
        

    } else {
      this.utils.showAlert("Erro", "Preencha corretamente os campos");
    }
  }
  
  private getValidAddress(): any {
    const formData = this.addressForm.value;

    return {
      Pessoa: {
        Id: HttpProvider.userAuth.PessoaId
      },	  
      Id: this.enderecoId,
      Descricao: formData.descricao,
        Tipo: 1,
      Cep: formData.cep,
      Logradouro: formData.logradouro,
      Numero: formData.numero,
      Complemento: formData.complemento,
      Bairro: formData.bairro,
      Cidade: {
        Id: formData.cidadeSelected,
        Estado: {
          Id: formData.estadoSelected,
          Pais: {
            Id: formData.paisSelected
          }
        }
      }
    }
  }

  obterEndereco() {
    this.geo.getCurrentPosition({
      timeout: 30000,
      enableHighAccuracy: true
    }).then(
      resp => {
        if (resp && resp.coords) {
          this.geolocationProvider.getAdressByGeolocation(resp.coords.latitude, resp.coords.longitude)
            .subscribe(
              retorno => {
                if (retorno.results.length > 0) {
                  this.addressForm.get('logradouro').setValue(retorno.results[0].address_components[1].long_name ? retorno.results[0].address_components[1].long_name : "");
                  this.addressForm.get('numero').setValue(retorno.results[0].address_components[0].long_name ? retorno.results[0].address_components[0].long_name : "");
                  this.addressForm.get('bairro').setValue(retorno.results[0].address_components[2].long_name ? retorno.results[0].address_components[2].long_name : "");
                  this.addressForm.get('cep').setValue(retorno.results[0].address_components[6].long_name ? retorno.results[0].address_components[6].long_name : "");

                  if (retorno.results[0].address_components[4].short_name)
                    this.peopleProvider.getCidades(this.estados.find(y => y.Sigla == retorno.results[0].address_components[4].short_name).Id)
                      .subscribe(
                        cidades => {
                          this.cidades = cidades || [];

                          this.addressForm.get('paisSelected').setValue(this.cidades.find(x => x.Descricao == retorno.results[0].address_components[3].long_name).Estado.Pais.Id);
                          this.addressForm.get('cidadeSelected').setValue(this.cidades.find(x => x.Descricao == retorno.results[0].address_components[3].long_name).Id);
                          this.addressForm.get('estadoSelected').setValue(this.cidades.find(x => x.Descricao == retorno.results[0].address_components[3].long_name).Estado.Id);
                        },
                        error => this.utils.showToast(error)
                      )
                }
              });
        }
        else {
          this.utils.hideLoader();
          this.utils.showToast("erro ao consultar o gps");
        }
      }
    ).catch((pe: PositionError) => {
      this.utils.hideLoader();
    });
  }
}
