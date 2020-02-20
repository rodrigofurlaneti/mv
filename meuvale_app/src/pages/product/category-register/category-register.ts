import { Component } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { ProductProvider } from '../../../providers/product/product';

@IonicPage()
@Component({
  selector: 'page-category-register',
  templateUrl: 'category-register.html'
})
export class CategoryRegisterPage {

  categoryForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      nome: new FormControl('', [Validators.required])
    }
  );
  logo: any;
  categorias: any[] = [];

  constructor(
    private utils: UtilsProvider,
    private navCtrl: NavController,
    private productProvider: ProductProvider
  ) {

    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para cadastrar uma Categoria');
      this.navCtrl.setRoot("LoginPage");
      return;
    }
  }

  ionViewDidLoad() {
    this.loadData();
  }

  loadData() {
    this.productProvider.listCategorias()
      .subscribe(
        data => { this.categorias = data; this.utils.hideLoader() },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error) }
      )
  }

  prepareForm(data: any): void {
    console.log(data);
    if (data) {
      this.categoryForm.setValue(
        {
          id: data.Id,
          nome: data.Nome
        }
      )
      this.logo = data.Logo;
    }
    this.utils.hideLoader();
  }

  dismiss() {
    this.navCtrl.pop();
  }

  doSubmit() {
    if (this.categoryForm.valid) {
      this.utils.showLoader("Registrando os dados...");

      let _data = null;

      _data = this.formToUserData();

      if (HttpProvider.userAuth) {
        this.productProvider.setCategoria(_data)
          .subscribe(data => {
            this.utils.hideLoader();
            this.utils.showAlert("Sucesso", "Categoria atualizada com sucesso!");
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

    const _formData = this.categoryForm.value;
    let data: any

    data = {
      Id: _formData.id,
      Nome: _formData.nome,
      LogoUpload: this.logo
    };

    return data;
  }

  edit(categoria) {
    this.prepareForm(categoria);
  }

  trash(categoria) {
    this.productProvider.deleteCategoria(categoria.Id).subscribe(data => {
      this.utils.showToast("Registro excluído com sucesso!");
      this.loadData();
    },
      error => {
        this.utils.showToast("Erro: Categoria possui registro vínculado!" + error.error.Message ? error.error.Message : error.error);
      })
      ;
  }
}
