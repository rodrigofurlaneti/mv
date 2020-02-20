import { Component } from '@angular/core';
import { IonicPage, NavParams, NavController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { ProductProvider } from '../../../providers/product/product';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpProvider } from '../../../providers/http/http';
import { FornecedorProvider } from '../../../providers/fornecedor/fornecedor';
import { StoreProvider } from '../../../providers/store/store';
import { ProductLoja } from '../../../model/product-loja';

/**
 * Generated class for the ProductRegisterPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-product-register',
  templateUrl: 'product-register.html',
})
export class ProductRegisterPage {
  produtoMv: any;
  produtoPreco: any;
  loja: any;
  categorias: any[] = [];
  departamentos: any[] = [];
  date = new Date().getDate().toString() + new Date().getMonth().toString() + new Date().getFullYear().toString()
    + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString();
  admUsr = HttpProvider.userAuth.PerfilId == "99" || HttpProvider.userAuth.PerfilId == "4";
  fornecedor: boolean = false;
  detalhe: string = "";
  detalhes: any[] = [];
  tipoDescontoSelected: number;

  productForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      foto: new FormControl(''),
      categoriaSelected: new FormControl('', [Validators.required]),
      departamentoSelected: new FormControl('', [Validators.required]),
      idproduto: new FormControl(''),
      nome: new FormControl('', [Validators.required]),
      descricao: new FormControl('', [Validators.required]),
      codigobarras: new FormControl(''),
      codigo: new FormControl(''),
      precovenda: new FormControl('', [Validators.required]),
      precodesconto: new FormControl(''),
      quantidadeestoque: new FormControl(''),
      inicioVigencia: new FormControl('', [Validators.required]),
      fimVigencia: new FormControl('', [Validators.required]),
      status: new FormControl(''),
      termo: new FormControl(''),
      codigoDesconto: new FormControl(''),
      linkDesconto: new FormControl('')
    }
  );

  constructor(private productProvider: ProductProvider,
    private utils: UtilsProvider,
    private navParams: NavParams,
    public navCtrl: NavController,
    private storeProvider: StoreProvider,
    private fornecedorProvider: FornecedorProvider) {
    this.produtoPreco = navParams.get("produto");
    this.loja = navParams.get("loja");
    if (this.navParams.get("fornecedor"))
      this.fornecedor = this.navParams.get("fornecedor");
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

    if (this.produtoPreco) {
      this.produtoMv = this.produtoPreco.Produto;
      this.prepareForm(this.produtoPreco);
    } else{
      this.produtoMv = new ProductLoja();
    }

    if (this.loja && this.produtoMv && this.produtoMv.Id > 0 && !this.produtoPreco)
      this.fornecedorProvider.getProdutoPorCodigoFornecedor(this.loja.Entidade.Id, this.produtoMv.Produto.Id).subscribe(
        data => {
          this.produtoPreco = data;
          this.prepareForm(this.produtoPreco);
          if (!this.produtoPreco)
            this.utils.showAlert("atenção", "Não temos este produto cadastrado na base");
        }, error => {
          this.utils.showToast(error.error);
        }
      )
    else
      this.productForm.controls["quantidadeestoque"].setValue(1);
  }

  handleEvent(e) {
    this.productForm.value.foto = e.URL;
    this.produtoMv.Imagens.push({ Descricao: e.URL });
    this.doSubmit();
    if(this.produtoMv.Id > 0){
    this.productProvider.getProdutoPorCodigo(this.produtoMv.Id).subscribe(
      data => {
        this.produtoMv = data;
      }, error => {
        this.utils.showToast(error.error);
      });
    }
  }

  onChangeCategoria() {
    this.productProvider.listDepartamentosPorCategoria(this.productForm.value.categoriaSelected)
      .subscribe(
        data => {
          this.departamentos = data; this.utils.hideLoader();
          this.onChangeDepartamento();
        },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error) }
      )
  }

  onChangeDepartamento() {
    let depto = this.departamentos.find(x => x.Id == this.productForm.value.departamentoSelected);
    if (depto && depto.LogoUpload) {
      this.productForm.value.foto = depto.LogoUpload;

      if(this.produtoMv)
      this.produtoMv.Imagens = [{ Descricao: depto.LogoUpload }];
    }
  }

  prepareForm(data: any): void {
    console.log(data);
    if (data) {
      this.productForm.setValue(
        {
          id: data.Id,
          foto: data.Produto.Imagens[0] ? data.Produto.Imagens[0].Descricao : data.Produto.DepartamentoProduto.LogoUpload,
          idproduto: data.Produto.Id,
          nome: data.Produto.Nome,
          descricao: data.Produto.Descricao,
          codigobarras: data.Produto.CodigoBarras,
          codigo: data.Produto.Codigo,
          categoriaSelected: data.Produto.DepartamentoProduto.CategoriaProduto.Id,
          departamentoSelected: data.Produto.DepartamentoProduto.Id,
          precovenda: data.Valor.toFixed(2),
          precodesconto: data.ValorDesconto.toFixed(2),
          quantidadeestoque: data.Quantidade == 0 ? 1 : data.Quantidade,
          inicioVigencia: data.InicioVigencia,
          fimVigencia: data.FimVigencia,
          status: data.Status,
          termo: data.Produto.Descricoes[0] ? data.Produto.Descricoes[0].Tipo == 3 ? data.Produto.Descricoes[0].Descricao : '' : '',
          codigoDesconto: !data.CodigoDesconto ? '': data.CodigoDesconto,
          linkDesconto: !data.LinkDesconto ? '': data.LinkDesconto
        }
      )
      this.detalhes = data.Produto.Descricoes && data.Produto.Descricoes.length > 0 ? data.Produto.Descricoes.filter(x => x.Tipo == 2) : [];
      this.productForm.value.categoriaSelected = data.Produto.CategoriaProduto.Id;
      this.onChangeCategoria();
      this.productForm.value.departamentoSelected = data.Produto.DepartamentoProduto.Id;
    }
    this.utils.hideLoader();
    console.log(data);
  }

  private formToUserData(): any {

    const _formData = this.productForm.value;
    let data: any

    data = {
      Id: _formData.id,
      Produto: {
        Id: _formData.idproduto,
        Nome: _formData.nome,
        Descricao: _formData.descricao,
        Informacoes: [{
          Tipo: 1,
          Descricao: _formData.foto
        }],
        CodigoBarras: _formData.codigobarras,
        Codigo: _formData.codigo,
        CategoriaProduto: {
          Id: _formData.categoriaSelected
        },
        DepartamentoProduto: {
          Id: _formData.departamentoSelected
        }
      },
      Valor: _formData.precovenda.replace(',', '.'),
      ValorDesconto: _formData.precodesconto.replace(',', '.'),
      Quantidade: _formData.quantidadeestoque,
      InicioVigencia: this.utils.dataToFromBody(_formData.inicioVigencia),
      FimVigencia: this.utils.dataToFromBody(_formData.fimVigencia),
      Status: _formData.status,
      CodigoDesconto: _formData.codigoDesconto,
      LinkDesconto: _formData.linkDesconto
    };

    this.detalhes.forEach(element => {
      let detalhe = {
            Tipo: 2,
            Descricao: element.Descricao,
          };
          data.Produto.Informacoes.push(detalhe);
    });

    if (_formData.termo && _formData.termo != '') {
      let termos = {
        Tipo: 3,
        Descricao: _formData.termo,
      };
      data.Produto.Informacoes.push(termos);
    }

    if (this.fornecedor == true)
      data.Fornecedor = this.loja;
    else
      data.Loja = this.loja;

    return data;
  }

  adicionarDetalhe() {
    let detalhe = {
      Tipo: 2,
      Descricao: this.detalhe,
    };
    this.detalhes.push(detalhe);
    this.detalhe = '';
  }

  removerDetalhe(ben) {
    var index = this.detalhes.indexOf(ben);
    this.detalhes.splice(index, 1);
  }

  doSubmit() {
    if (this.productForm.valid) {
      this.utils.showLoader("Registrando o produto para seu estabelecimento");

      let _data = null;

      _data = this.formToUserData();

      if (HttpProvider.userAuth) {
        if (this.fornecedor == true)
          this.fornecedorProvider.salvarProdutoPrecoFornecedor(this.loja.Id, _data)
            .subscribe(data => {
              this.utils.hideLoader();
              this.utils.showAlert("Sucesso", "Produto atualizado com sucesso!");
              this.navCtrl.pop();
            }, error => {
              this.utils.hideLoader();
              this.utils.showToast(error.error);
            });
        else
          this.storeProvider.salvarProdutoPrecoLoja(this.loja.Id, _data)
            .subscribe(data => {
              this.utils.hideLoader();
              this.utils.showToast("Produto atualizado com sucesso!");
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
}
