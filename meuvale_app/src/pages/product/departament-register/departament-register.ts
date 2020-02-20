import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { ProductProvider } from '../../../providers/product/product';

@IonicPage()
@Component({
  selector: 'page-departament-register',
  templateUrl: 'departament-register.html'
})
export class DepartamentRegisterPage {

  departamentForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      nome: new FormControl('', [Validators.required]),
      categoriaSelected: new FormControl('', [Validators.required]),
      logoUpload: new FormControl('')
    }
  );
  departamentos: any[] = [];
  categorias: any[] = [];
  date = new Date().getDate();

  constructor(
    private utils: UtilsProvider,
    private navCtrl: NavController,
    private productProvider: ProductProvider
  ) {

    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para cadastrar uma Departamento');
      this.navCtrl.setRoot("LoginPage");
      return;
    }
  }

  ionViewDidLoad() {
    this.loadData();
  }

  handleEvent(e) {
    this.departamentForm.value.logoUpload = e.URL;

    this.doSubmit();
  }

  loadData() {
    this.productProvider.listDepartamentos()
      .subscribe(
        data => { this.departamentos = data; this.utils.hideLoader() },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error) }
      )

      this.productProvider.listCategorias()
      .subscribe(
        data => { this.categorias = data; this.utils.hideLoader() },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error) }
      )
  }

  prepareForm(data: any): void {
    console.log(data);
    if (data) {
      this.departamentForm.setValue(
        {
          id: data.Id,
          nome: data.Nome,
          categoriaSelected: data.CategoriaProduto.Id,
          logoUpload: data.LogoUpload
        }
      )
    }
    this.utils.hideLoader();
  }

  dismiss() {
    this.navCtrl.pop();
  }

  doSubmit() {
    if (this.departamentForm.valid) {
      this.utils.showLoader("Registrando os dados...");

      let _data = null;

      _data = this.formToUserData();

      if (HttpProvider.userAuth) {
        this.productProvider.setDepartamento(_data)
          .subscribe(data => {
            this.utils.hideLoader();
            this.utils.showAlert("Sucesso", "Departamento atualizado com sucesso!");
            this.loadData();
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

    const _formData = this.departamentForm.value;
    let data: any

    data = {
      Id: _formData.id,
      Nome: _formData.nome,
      CategoriaProduto: this.categorias.find(x => x.Id == _formData.categoriaSelected),
      LogoUpload: _formData.logoUpload
    };

    return data;
  }

  edit(Departamento) {
    this.prepareForm(Departamento);
  }

  trash(departamento) {
    this.productProvider.deleteDepartamento(departamento.Id).subscribe(data => {
      this.utils.showToast("Registro excluído com sucesso!");
      this.loadData();
    },
      error => {
        this.utils.showToast(error.error.Message ? error.error.Message : error.error);
      })
      ;
  }
}
