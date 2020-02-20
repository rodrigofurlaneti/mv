import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavParams, NavController, ModalController } from 'ionic-angular';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoreProvider } from '../../../providers/store/store';
import { PeopleProvider } from '../../../providers/people/people';
import { HttpProvider } from '../../../providers/http/http';
import { GeolocationProvider } from '../../../providers/google/geolocation';
import { CepProvider } from '../../../providers/adress/cep';
import { PageProvider } from '../../../providers/page/page';
import { ProductProvider } from '../../../providers/product/product';
import { BankProvider } from '../../../providers/bank/bank';
import { TerminaisLoja, Terminal } from '../../../model/terminaisLoja';
import { SignaturePad } from 'angular2-signaturepad/signature-pad';

@IonicPage()
@Component({
  selector: 'page-store-register-full',
  templateUrl: 'store-register-full.html'
})
export class StoreRegisterFullPage {
  storeForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      cnpj: new FormControl('', [Validators.required]),
      dataAceiteContrato: new FormControl(''),
      ie: new FormControl(''),
      nomefantasia: new FormControl('', [Validators.required]),
      razaosocial: new FormControl('', [Validators.required]),
      telefone: new FormControl('', [Validators.required]),
      celular: new FormControl(''),
      email: new FormControl('', [Validators.required, Validators.email]),
      idEndereco: new FormControl(''),
      classificacao: new FormControl('', [Validators.required]),
      classificacaoOutros: new FormControl(''),
      cep: new FormControl('', [Validators.required]),
      logradouro: new FormControl('', [Validators.required]),
      numero: new FormControl('', [Validators.required]),
      complemento: new FormControl(),
      paisSelected: new FormControl('', [Validators.required]),
      estadoSelected: new FormControl('', [Validators.required]),
      cidadeSelected: new FormControl('', [Validators.required]),
      ativo: new FormControl(''),
      delivery: new FormControl(''),
      dataAceiteDelivery: new FormControl(''),
      raioAtendimento: new FormControl(''),
      fotoFachada: new FormControl(''),
      codigoInfox: new FormControl('')
    }
  );

  proprietarioForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      cpf: new FormControl('', [Validators.required]),
      rg: new FormControl(''),
      nome: new FormControl('', [Validators.required]),
      sexo: new FormControl('', [Validators.required]),
      dataNascimento: new FormControl('', [Validators.required]),
      celular: new FormControl(''),
      email: new FormControl('')
    });

  bankForm: FormGroup = new FormGroup(
    {
      id: new FormControl(''),
      banco: new FormControl(''),
      agencia: new FormControl(''),
      digitoAgencia: new FormControl(''),
      conta: new FormControl(''),
      digitoConta: new FormControl(''),
      documentoTitular: new FormControl(''),
      nomeTitular: new FormControl(''),
    });

  produtoAtivacaoLojaForm: FormGroup = new FormGroup({
    id: new FormControl(''),
    alimentacao: new FormControl(''),
    refeicao: new FormControl(''),
    adiantamento: new FormControl(''),
    combustivel: new FormControl(''),
    farmacia: new FormControl(''),
    taxaRefeicao: new FormControl(''),
    taxaAlimentacao: new FormControl(''),
    taxaCombustivel: new FormControl(''),
    taxaAdiantamento: new FormControl(''),
  })

  machineForm: FormGroup = new FormGroup({
    id: new FormControl(''),
    idMaquininha: new FormControl(''),
    maquininha: new FormControl(''),
    gerenciadora: new FormControl(''),
    softwareHouse: new FormControl(''),
    modelo: new FormControl(''),
    numeroSerial: new FormControl(''),
    taxaCredito: new FormControl(''),
    taxaDebito: new FormControl('')
  })

  paises: any[] = [];
  estados: any[] = [];
  cidades: any[] = [];
  categorias: any[] = [];
  bancos: any[] = [];
  produtos = [{ Nome: "Alimentação", Ativo: false },
  { Nome: "Refeição", Ativo: false },
  { Nome: "Adiantamento Salarial", Ativo: false },
  { Nome: "Combustível", Ativo: false },
  { Nome: "Farmácia", Ativo: false }];
  maquininhas = [{ Id: 1, Descricao: "Rede Vale", Ativo: false },
  { Id: 2, Descricao: "Cielo", Ativo: false },
  { Id: 4, Descricao: "Aplicativo", Ativo: false },
  { Id: 5, Descricao: "TEFF", Ativo: false },
  { Id: 6, Descricao: "POS", Ativo: false },
  { Id: 7, Descricao: "TODAS", Ativo: false }];
  maquinasLoja: TerminaisLoja[] = [];
  loja: any;
  latitude: any;
  logintude: any;
  lojaAprovada: false;
  date = new Date().getDate().toString() + new Date().getMonth().toString() + new Date().getFullYear().toString()
    + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString();
  faseCadastro = 1;
  exibeCampoTipoOutrosEstabelecimento = false;
  habilitaBotaoAceitar: boolean = false;
  sendedEtapa4: boolean = false;
  sendedEtapa3: boolean = false;
  sendedEtapa5: boolean = false;
  usuarioLogado: boolean = false;

  signature = '';
  isDrawing = false;
  @ViewChild(SignaturePad) signaturePad: SignaturePad;
  private signaturePadOptions: Object = { // Check out https://github.com/szimek/signature_pad
    'minWidth': 2,
    'canvasWidth': 400,
    'canvasHeight': 200,
    'backgroundColor': '#f6fbff',
    'penColor': '#666a73'
  };

  constructor(
    private utils: UtilsProvider,
    private peopleProvider: PeopleProvider,
    private navParams: NavParams,
    private navCtrl: NavController,
    private storeProvider: StoreProvider,
    private geolocationProvider: GeolocationProvider,
    private cepProvider: CepProvider,
    private mdl: ModalController,
    private productProvider: ProductProvider,
    private banckProvider: BankProvider
    //,private signaturePad : SignaturePad
    ) {
    this.loja = this.navParams.get("loja");
    if (this.loja) {
      this.faseCadastro = this.loja.FaseCadastro == 0 ? 1 : this.loja.FaseCadastro;
      this.habilitaBotaoAceitar = this.faseCadastro >= 1;
    }

    this.storeForm.value.delivery = false;

    if (navParams.get("cnpj"))
      this.storeForm.value.cnpj = navParams.get("cnpj");

    this.usuarioLogado = HttpProvider.userAuth ? true : false;
  }

  canvasResize() {
    let canvas = document.querySelector('canvas');
    this.signaturePad.resizeCanvas();
    this.signaturePad.set('minWidth', 1);
    this.signaturePad.set('canvasWidth', canvas.offsetWidth);
    this.signaturePad.set('canvasHeight', canvas.offsetHeight);
    }

  ionViewDidLoad() {
    // this.signaturePad.clear();
    // this.canvasResize();

    if (HttpProvider.userAuth) {
      if (this.loja) {
        this.utils.showLoader("Carregando seu estabelecimento");
        this.storeProvider.getLoja(this.loja.Id)
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

    this.productProvider.listCategorias()
      .subscribe(
        data => { this.categorias = data; this.utils.hideLoader() },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error) }
      );

    this.banckProvider.loadBanks()
      .subscribe(
        data => { this.bancos = data; this.utils.hideLoader() },
        error => { this.utils.hideLoader(); this.utils.showToast(error.error) }
      );
  }

  handleEvent(e) {
    if (this.faseCadastro == 8) {
      this.loja.FotoFachada = e.URL;
    }
    else {
      if (!this.loja)
        this.loja = { Logo: e.URL }
      else
        this.loja.Logo = e.URL;

      this.loja.LogoUpload = e.URL;
    }
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
    console.log(data);
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
          idEndereco: data.Endereco ? data.Endereco.Id : 0,
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
          delivery: data.Delivery,
          classificacao: data.LojaTipoLoja,
          classificacaoOutros: data.LojaTipoLojaOutros,
          raioAtendimento: data.RaioAtendimento,
          dataAceiteContrato: data.DataAceiteContrato,
          dataAceiteDelivery: data.DataAceiteContratoDelivery,
          fotoFachada: data.FotoFachada,
          codigoInfox: data.CodigoInfox
        }
      );

      this.proprietarioForm.setValue({
        id: data.Proprietario ? data.Proprietario.Id : 0,
        cpf: data.Proprietario ? data.Proprietario.Cpf : "",
        rg: data.Proprietario ? data.Proprietario.Rg : "",
        nome: data.Proprietario ? data.Proprietario.Nome : "",
        sexo: data.Proprietario ? data.Proprietario.Sexo : "",
        dataNascimento: data.Proprietario ? this.utils.formatData(data.Proprietario.DataNascimento) : "",
        celular: data.Proprietario ? data.Proprietario.Celular : "",
        email: data.Proprietario ? data.Proprietario.Email : "",
      });

      this.prepareFormBankLoja(data);

      this.prepareFormProdutoAtivacaoLoja(data);

      this.maquinasLoja = data.TerminalCobrancaLojas ? data.TerminalCobrancaLojas : [] = [];

      this.loja.Logo = data.Logo;
      this.lojaAprovada = data.LojaAprovada;
      this.onChangeEstado();
      this.storeForm.value.cidadeSelected = data.Endereco ? data.Endereco.Cidade ? data.Endereco.Cidade.Id : '' : '';
      this.exibeCampoTipoOutrosEstabelecimento = data.Classificacao == "Outros";
    }
    this.utils.hideLoader();
    console.log(data);
  }

  dismiss() {
    this.navCtrl.setRoot("StoreListPage")
  }

  doSubmit() {
    if (this.faseCadastro >= 3) {
      this.sendedEtapa3 = true;
      if (this.storeForm.valid) {
        if (this.faseCadastro == 4) {
          this.sendedEtapa4 = true;
          if (!this.proprietarioForm.valid) {
            this.utils.showToast("Preencha os campos corretamente!");
            return;
          }
        }

        if (this.faseCadastro == 5) {
          this.sendedEtapa5 = true;
          if (!this.bankForm.valid) {
            this.utils.showToast("Preencha os campos corretamente!");
            return;
          }
        }

        if (this.faseCadastro == 8)
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

              if (this.faseCadastro == 7) {
                _data.Assinatura = this.signature;
              }

              this.storeProvider.salvarLoja(_data)
                .subscribe(data => {
                  if (this.faseCadastro >= 3) {
                    this.storeProvider.getLojaByCNPJ(_data.Cnpj)
                      .subscribe(
                        data => {
                          if (data) {
                            this.loja = data;
                            this.prepareForm(data);
                            if (data.FaseCadastro)
                              this.faseCadastro = data.FaseCadastro;
                          }
                        }
                      )
                  }
                  if (this.faseCadastro == 8) {
                    this.utils.hideLoader();
                    this.utils.showAlert("Sucesso", "Estabelecimento atualizado com sucesso!");
                  }
                }, error => {
                  this.utils.hideLoader();
                  this.utils.showToast(error.error);
                });
            }
          )
      }
      else {
        this.utils.showToast("Preencha os campos corretamente!");
        return;
      }
    }
    this.faseCadastro += 1;
  }

  private formToUserData(): any {

    const _formData = this.storeForm.value;
    let data: any;
    let pesssoa = this.proprietarioFormToUserData();
    let dadosBancarios = this.bankFormToUserData();
    let produtoAtivacaoLoja = this.produtoAtivacaoLojaFormToUserData();

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
      LogoUpload: this.loja && this.loja.Logo ? this.loja.Logo : "assets/image/logo.png",
      Proprietario: pesssoa,
      AceiteContrato: this.faseCadastro > 1,
      DataAceiteContrato: !_formData.dataAceiteContrato || _formData.dataAceiteContrato == "" || _formData.dataAceiteContrato == "0001-01-01T00:00:00" ?
        this.utils.dataToFromBody(new Date().getDate().toString() + "/" + new Date().getMonth().toString() + "/" + new Date().getFullYear().toString())
        : _formData.dataAceiteContrato,
      FaseCadastro: this.faseCadastro,
      Delivery: _formData.delivery,
      AceiteContratoDelivery: _formData.delivery == true,
      DataAceiteContratoDelivery: !_formData.dataAceiteDelivery || _formData.dataAceiteDelivery == "" || _formData.dataAceiteDelivery == "0001-01-01T00:00:00" ?
        this.utils.dataToFromBody(new Date().getDate().toString() + "/" + new Date().getMonth().toString() + "/" + new Date().getFullYear().toString())
        : _formData.dataAceiteDelivery,
      LojaTipoLoja: [{
        TipoLoja: {
          Descricao: _formData.classificacao != null ?
            _formData.classificacao : _formData.classificacaoOutros
        }
      }],
      RaioAtendimento: _formData.raioAtendimento,
      DadosBancarios: dadosBancarios,
      FotoFachada: _formData.fotoFachada,
      TerminaisLoja: this.maquinasLoja,
      CodigoInfox: _formData.codigoInfox,
      ProdutoAtivacaoLoja: produtoAtivacaoLoja
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

  voltarFase(fase = 0) {
    if (fase == 0)
      this.faseCadastro -= 1;
    else
      this.faseCadastro = fase;
  }

  search(event: any) {
    this.utils.showLoader("Buscando CNPJ na base...")
    var cnpj = event.target.value;

    this.storeProvider.getLojaByCNPJ(cnpj)
      .subscribe(
        data => {
          if (data) {
            this.loja = data;
            this.prepareForm(data);
            if (data.FaseCadastro)
              this.faseCadastro = data.FaseCadastro;
            this.utils.showToast("Cadastro encontrado!");
            this.utils.hideLoader();
          }
          else {
            this.utils.showToast("CNPJ não cadastrado, favor continuar com o cadastro...")
            this.utils.hideLoader();
          }
        },
        error => {
          this.utils.showToast("CNPJ não cadastrado, favor continuar com o cadastro...")
          this.utils.hideLoader();
        }
      )

    this.habilitaBotaoAceitar = cnpj != "" && (cnpj.length == 18 || cnpj.length == 14);
  }

  private bankFormToUserData(): any {
    const _formData = this.bankForm.value;

    let bankLoja: any = {
      Id: _formData.id,
      Banco: { Id: _formData.banco },
      Agencia: _formData.agencia,
      DigitoAgencica: _formData.digitoAgencia,
      Conta: _formData.conta,
      DigitoConta: _formData.digitoConta,
      DocumentoTitular: _formData.documentoTitular,
      NomeTitular: _formData.nomeTitular
    };

    return bankLoja;
  }

  private proprietarioFormToUserData(): any {
    const _formData = this.proprietarioForm.value;

    const _pessoa: any = {
      Id: _formData.id
    }

    let Pessoa: any = {
      Id: _formData.id,
      Nome: _formData.nome,
      DataNascimento: this.utils.dataToFromBody(_formData.dataNascimento),
      Sexo: _formData.sexo,
      Documentos: [
        {
          Numero: _formData.cpf,
          Tipo: 2,
          Id: 0,
          Pessoa: _pessoa
        },
        {
          Numero: _formData.rg,
          Tipo: 1,
          Id: 0,
          Pessoa: _pessoa
        }
      ],
      Contatos: [
        {
          Contato: {
            Email: _formData.email,
            Tipo: 1,
            Id: 0,
            Pessoa: _pessoa
          }
        },
        {
          Contato: {
            Numero: _formData.celular,
            Tipo: 3,
            Id: 0,
            Pessoa: _pessoa
          }
        }
      ]
    }
    return Pessoa;
  }

  private machineFormToUserData(): any {
    const _formData = this.machineForm.value;

    let machineLoja: TerminaisLoja = {
      Id: _formData.id,
      Loja: { Id: this.loja.Id },
      Usuario: { Id: HttpProvider.userAuth.UsuarioId },
      Terminal: {
        Id: _formData.idMaquininha,
        Maquininha: _formData.maquininha,
        NomeGerenciadora: _formData.gerenciadora,
        Modelo: _formData.modelo,
        NumeroSerial: _formData.numeroSerial,
        SoftwareHouse: _formData.softwareHouse,
        TaxaCredito: _formData.taxaCredito,
        TaxaDebito: _formData.taxaDebito,
      }
    };

    return machineLoja;
  }

  private produtoAtivacaoLojaFormToUserData(): any {
    const _formData = this.produtoAtivacaoLojaForm.value;

    let produtoAtivacaoLoja = {
      Id: _formData.id == "" ? 0 : _formData.id,
      ProdutoAlimentacao: _formData.alimentacao.toString() == "false" ? false : true,
      ProdutoRefeicao: _formData.refeicao.toString() == "false" ? false : true,
      ProdutoAdiantamento: _formData.adiantamento.toString() == "false" ? false : true,
      ProdutoCombustivel: _formData.combustivel.toString() == "false" ? false : true,
      ProdutoFarmacia: _formData.farmacia.toString() == "false" ? false : true,
      TaxaRefeicao: Number.parseFloat(_formData.taxaRefeicao.replace(',', '.')),
      TaxaAlimentacao: Number.parseFloat(_formData.taxaAlimentacao.replace(',', '.')),
      TaxaCombustivel: Number.parseFloat(_formData.taxaCombustivel.replace(',', '.')),
      TaxaAdiantamento: Number.parseFloat(_formData.taxaAdiantamento.replace(',', '.'))
    };

    return produtoAtivacaoLoja;
  }

  private prepareFormBankLoja(data: any): void {
    if (data.DadosBancarios) {
      this.bankForm.setValue(
        {
          id: data ? data.DadosBancarios.Id : 0,
          banco: data ? data.DadosBancarios.Banco : 0,
          agencia: data ? data.DadosBancarios.Agencia : "",
          digitoAgencia: data ? data.DadosBancarios.DigitoAgencica : "",
          conta: data ? data.DadosBancarios.Conta : "",
          digitoConta: data ? data.DadosBancarios.DigitoConta : "",
          documentoTitular: data ? data.DadosBancarios.DocumentoTitular : "",
          nomeTitular: data ? data.DadosBancarios.NomeTitular : "",
        }
      )
    }
  }

  private prepareFormProprietario(data: any): void {
    if (data) {
      this.proprietarioForm.setValue(
        {
          id: data ? data.Id : 0,
          cpf: data ? data.Cpf : "",
          rg: data ? data.Rg : "",
          nome: data ? data.Nome : "",
          sexo: data ? data.Sexo : "",
          dataNascimento: data ? this.utils.formatData(data.DataNascimento) : "",
          celular: data ? data.Celular : "",
          email: data ? data.Email : ""
        }
      )
    }
  }

  private prepareFormProdutoAtivacaoLoja(data: any): void {
    if (data.ProdutoAtivacaoLoja) {
      this.produtoAtivacaoLojaForm.setValue(
        {
          id: data ? data.ProdutoAtivacaoLoja.Id : 0,
          alimentacao: data ? data.ProdutoAtivacaoLoja.ProdutoAlimentacao : "",
          refeicao: data ? data.ProdutoAtivacaoLoja.ProdutoRefeicao : "",
          adiantamento: data ? data.ProdutoAtivacaoLoja.ProdutoAdiantamento : "",
          combustivel: data ? data.ProdutoAtivacaoLoja.ProdutoCombustivel : "",
          farmacia: data ? data.ProdutoAtivacaoLoja.ProdutoFarmacia : "",
          taxaRefeicao: data ? data.ProdutoAtivacaoLoja.TaxaRefeicao.toFixed(2) : 0,
          taxaAlimentacao: data ? data.ProdutoAtivacaoLoja.TaxaAlimentacao.toFixed(2) : 0,
          taxaCombustivel: data ? data.ProdutoAtivacaoLoja.TaxaCombustivel.toFixed(2) : 0,
          taxaAdiantamento: data ? data.ProdutoAtivacaoLoja.TaxaAdiantamento.toFixed(2) : 0
        }
      )
    }
  }

  private prepareFormMachine(data: any): void {
    if (data) {
      this.machineForm.setValue(
        {
          id: data ? data.Id : 0,
          idMaquininha: data ? data.Terminal.Id : "",
          maquininha: data ? data.Terminal.Maquininha : "",
          gerenciadora: data ? data.Terminal.NomeGerenciadora : "",
          softwareHouse: data ? data.Terminal.SoftwareHouse : "",
          modelo: data ? data.Terminal.Modelo : "",
          numeroSerial: data ? data.Terminal.NumeroSerial : "",
          taxaCredito: data ? data.Terminal.TaxaCredito : 0,
          taxaDebito: data ? data.Terminal.TaxaDebito : 0
        }
      )
    }
  }

  searchFunc(event: any) {
    this.utils.showLoader("Verificando CPF na base...")
    var cpf = event.target.value;

    this.peopleProvider.getPessoaByCPF(cpf)
      .subscribe(
        data => {
          if (data) {
            if (event.target.parentElement.id == "cpfProprietario") {
              this.prepareFormProprietario(data);
            }

            this.utils.showToast("Cadastro encontrado!");
            this.utils.hideLoader();
          }
          else {
            this.utils.showToast("CPF não cadastrado, favor continuar o cadastro...")
            this.utils.hideLoader();
          }
        },
        error => {
          this.utils.showToast("CPF não cadastrado, favor continuar o cadastro...")
          this.utils.hideLoader();
        }
      )
  }

  contract() {
    this.mdl.create("ContractPage", {
      "cnpj": this.storeForm.value.cnpj,
      "telaOrigem": PageProvider._pageSelected,
      "tabOrigem": PageProvider._tabSelected,
      modal: true,
      noClose: true
    })
      .present();
  }

  alteraClassificacao(data) {
    if (data == "Outros")
      this.exibeCampoTipoOutrosEstabelecimento = true;
    else {
      this.exibeCampoTipoOutrosEstabelecimento = false;
      this.storeForm.value.ClassificacaoOutros = "";
    }
  }

  novoCadastro() {
    this.navCtrl.push("StoreRegisterFullPage");
  }

  insertMachine() {
    if (this.machineForm.valid) {
      this.maquinasLoja.push(this.machineFormToUserData());
      this.machineForm.reset();
    }
    else {
      this.utils.showToast("Preencha os campos corretamente!");
      return;
    }
  }

  editMachine(machine) {
    this.maquinasLoja.splice(this.maquinasLoja
      .findIndex(x => x.Id == machine.Id), 1);
    this.prepareFormMachine(machine);
  }

  removeMachine(machine) {
    this.maquinasLoja.splice(this.maquinasLoja
      .findIndex(x => x.Id == machine.Id), 1);
  }

  qrCode() {
    this.navCtrl.push('StoreQrCodePage', { loja: this.loja })
  }

  drawComplete() {
    this.isDrawing = false;
  }
 
  drawStart() {
    this.isDrawing = true;
  }
 
  savePad() {
    this.signature = this.signaturePad.toDataURL();
    //this.storage.set('savedSignature', this.signature);
    this.signaturePad.clear();
    // let toast = this.toastCtrl.create({
    //   message: 'New Signature saved.',
    //   duration: 3000
    // });
    // toast.present();
  }
 
  clearPad() {
    this.signaturePad.clear();
  }
}
