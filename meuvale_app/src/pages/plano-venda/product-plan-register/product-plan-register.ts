import { Component } from '@angular/core';
import { IonicPage, NavParams, NavController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpProvider } from '../../../providers/http/http';
import { PeopleProvider } from '../../../providers/people/people';
import { ProductPlanProvider } from '../../../providers/product-plan/product-plan';
import { ConvenioProvider } from '../../../providers/convenio/convenio';
import { FirebaseUploaderComponent } from '../../../components/firebase-upload/uploader/uploader';

/**
 * Generated class for the ProductPlanRegisterPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-product-plan-register',
  templateUrl: 'product-plan-register.html',
})
export class ProductPlanRegisterPage {

  produtoMv: any;
  date = new Date().getDate().toString() + new Date().getMonth().toString() + new Date().getFullYear().toString()
    + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString();
  admUsr = HttpProvider.userAuth.PerfilId == "99";
  fornecedor: boolean = false;
  paises: any[] = [];
  estados: any[] = [];
  cidades: any[] = [];
  lojaPesquisa: string;
  lojasFilter: any[] = [];
  limparBusca: boolean = false;
  lojas: any[] = [];
  lojaSelecionada: any;
  paisSelected: any;
  estadoSelected: any;
  cidadeSelected: any;
  percentual: number;
  percentuais: number[] = [];
  beneficio: string;
  beneficios: string[] = [];
  fotos: string[] = [];
  fotosExcluidas: string[] = [];

  productForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      nome: new FormControl('', [Validators.required]),
      precovenda: new FormControl('', [Validators.required]),
      precodesconto: new FormControl(''),
      inicioVigencia: new FormControl('', [Validators.required]),
      fimVigencia: new FormControl('', [Validators.required]),
      status: new FormControl('')
    }
  );

  constructor(
    private utils: UtilsProvider,
    private navParams: NavParams,
    private navCtrl: NavController,
    private conveioProvider: ConvenioProvider,
    private productPlanProvider: ProductPlanProvider,
    private peopleProvider: PeopleProvider) {
    this.produtoMv = navParams.get("planoVenda");
  }

  ionViewDidLoad() {
    this.loadData();
  }

  loadData() {
    this.peopleProvider.getPaises()
      .subscribe(
        data => {
          this.paises = data || [];
          if (this.paises.length) {
            this.paisSelected = this.paises[0].Id
          }

          this.peopleProvider.getEstados()
            .subscribe(
              data => {
                this.estados = data || [];
              },
              error => this.utils.showToast(error)
            );
        },
        error => this.utils.showToast(error)
      );

    if (this.produtoMv) {
      this.prepareForm(this.produtoMv);
    }
  }

  handleEvent(e) {
    this.fotos.push(e.URL + ";" + e.FileName);
  }

  onChangeEstado() {
    this.peopleProvider.getCidades(this.estadoSelected)
      .subscribe(
        data => this.cidades = data || [],
        error => this.utils.showToast(error)
      )
  }

  search() {
    if (!this.lojaPesquisa || this.lojaPesquisa == "")
      this.lojasFilter = null;

    this.conveioProvider.getPorNomeDocumentoOuCidade(this.estadoSelected,
      this.cidadeSelected, "",
      this.lojaPesquisa)
      .subscribe(
        data => {
          this.lojasFilter = data;
        },
        error => {
          this.utils.showError(error.message);
        }
      )
  }

  retornaLojas(loja) {
    this.lojaSelecionada = loja;

    this.lojasFilter = null;
    this.lojaPesquisa = "";
    this.limparBusca = true;
  }

  limparBuscaLojas() {
    this.lojas = [];
    this.limparBusca = false;
    this.lojaSelecionada = null;
  }

  prepareForm(data: any): void {
    if (data) {
      this.productForm.setValue(
        {
          id: data.Id,
          nome: data.Nome,
          precovenda: data.Valor.toFixed(2),
          precodesconto: data.ValorDesconto.toFixed(2),
          inicioVigencia: data.DataInicio,
          fimVigencia: data.DataFim,
          status: data.Status
        }
      );
      this.lojaSelecionada = data.Convenio;
      this.percentuais = data.Percentuais;
      this.beneficios = data.Beneficios;
      this.fotos = data.Fotos;
    }
    this.utils.hideLoader();
  }

  private formToUserData(): any {

    const _formData = this.productForm.value;
    let data: any

    data = {
      Id: _formData.id,
      Nome: _formData.nome,
      Valor: _formData.precovenda.replace(',', '.'),
      ValorDesconto: _formData.precodesconto.replace(',', '.'),
      InicioVigencia: this.utils.dataToFromBody(_formData.inicioVigencia),
      FimVigencia: this.utils.dataToFromBody(_formData.fimVigencia),
      Status: _formData.status,
      Convenio: { Id: this.lojaSelecionada.Id },
      Percentuais: this.percentuais,
      Beneficios: this.beneficios,
      Fotos: this.fotos
    };

    return data;
  }

  doSubmit() {
    if (this.productForm.valid) {

      if (this.beneficios.length <= 0) {
        this.utils.showAlert("Erro", "Nenhum benefício adicionado a lista!");
        return;
      }

      this.utils.showLoader("Registrando o combo");

      let _data = null;

      _data = this.formToUserData();

      if (HttpProvider.userAuth) {
        this.productPlanProvider.save(_data)
          .subscribe(() => {
            for (let foto of this.fotosExcluidas) {
              FirebaseUploaderComponent.deleteFromCloudStorage('combo', foto);
            }
            this.utils.hideLoader();
            this.utils.showAlert("Sucesso", "Combo cadastrado com sucesso!");
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
  }

  adicionarPercentual() {
    this.percentuais.push(this.percentual);
    this.percentual = 0;
  }

  trashPerc(perc) {
    var index = this.percentuais.indexOf(perc);
    this.percentuais.splice(index, 1);
  }

  adicionarBeneficio() {
    this.beneficios.push(this.beneficio);
    this.beneficio = '';
  }

  trashBeneficio(ben) {
    var index = this.beneficios.indexOf(ben);
    this.beneficios.splice(index, 1);
  }

  trashFoto(foto) {
    var index = this.fotos.indexOf(foto);
    this.fotos.splice(index, 1);
    this.fotosExcluidas.push(foto.split(';')[1]);
  }
}