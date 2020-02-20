import { Injectable, EventEmitter } from '@angular/core';
import { HttpProvider } from '../http/http';

@Injectable()
export class StoreProvider {
  private static _storeSelected: any;
  public static readonly storeChange: EventEmitter<any> = new EventEmitter();
  
  constructor(private defaultHttp: HttpProvider) {}
  
  public static getCurrentStore(): any {
    return StoreProvider._storeSelected;
  }

  public static setCurrentStore(loja: any): any {
    StoreProvider._storeSelected = loja;
    StoreProvider.storeChange.emit(loja);
  }

  temAcesso(usuarioId) {
    let acesso = this.defaultHttp.request('get', 'api/clube/acesso?usuarioId=' + usuarioId, null, {
      observe: 'response'
    });
    return acesso;
  }

  lojas(lat, lng, inicio = 0, quantidade = 10) {
    let lojas = this.defaultHttp.request("get", 'api/loja/lojasProximas?latitude=' + lat + '&longitude=' + lng+ '&inicio=' + inicio + '&quantidade=' + quantidade);
    return lojas;
  }

  lojasPorClassificacao(lat, lng, inicio = 0, quantidade = 10, classificacao: string) {
    let lojas = this.defaultHttp.request("get", 'api/loja/lojasProximasPorClassificacao?latitude=' + lat + '&longitude=' + lng+ '&inicio=' + inicio + '&quantidade=' + quantidade + '&classificacao=' + classificacao);
    return lojas;
  }

  lojasUsuario(pessoaId, lat, lng) {
    return this.defaultHttp.request("get", 'api/loja/lojasUsuario?pessoa=' + pessoaId + '&latitude=' + lat + '&longitude=' + lng);
  }

  getPrecoPorLojaProduto(lojaId, produtoId) {
      let params = { lojaId: lojaId, produtoId: produtoId };
      return this.defaultHttp.request("get", 'api/preco', params);
  }

  getProdutos(lojaId, page: number = 0) {
    return this.defaultHttp.request("get", `api/loja/${lojaId}/produtos?inicio=${page}&quantidade=10`);
  }
  
  getProdutosFornecedor(fornecedorId) {
    return this.defaultHttp.request("get", 'api/fornecedor/' + fornecedorId);
  }

  getProdutosFornecedorPorUsuario(fornecedorId, usuarioId) {
    return this.defaultHttp.request("get", 'api/fornecedor/produtos/' + fornecedorId + '/' +  usuarioId)
  }

  getProdutoPorCodBarras(lojaId, codBarras) {
      return this.defaultHttp.request("get", 'api/loja/' + lojaId + '/produtoCodBarras?codBarras=' + codBarras);
  }

  getLoja(id) {
    return this.defaultHttp.request("get", 'api/loja/' + id);
  }

  getLojaByCNPJ(cnpj) {
    return this.defaultHttp.request("get", 'api/loja/buscaLojaPorCNPJ/' + cnpj.replace(".","").replace("-","").replace("/","").replace(".",""));
  }

  getFornecedor(id) {
    return this.defaultHttp.request("get", 'api/fornecedor/' + id);
  }

  getTiposClassif() {
    return this.defaultHttp.request("get", 'api/loja/tiposClassificacao');
  }

  getClassificacoes(tipo) {
    return this.defaultHttp.request("get", 'api/loja/classificacoes/' + tipo);
  }

  salvarLoja(loja) {

    var obj = {
      Id: loja.Id,
      Codigo: loja.Codigo,
      Descricao: loja.Descricao,
      Cnpj: loja.Cnpj,
      RazaoSocial: loja.RazaoSocial,
      InscricaoEstadual: loja.InscricaoEstadual,
      Classificacao: loja.Classificacao,
      Telefone: loja.Telefone,
      Celular: loja.Celular,
      Email: loja.Email,
      LojaAprovada: loja.LojaAprovada,
      Status: loja.Status,
      LogoUpload: loja.LogoUpload,
      Endereco: {},
      Proprietario: loja.Proprietario,
      AceiteContrato: loja.AceiteContrato,
      DataAceiteContrato: loja.DataAceiteContrato,
      FaseCadastro: loja.FaseCadastro,
      Delivery: loja.Delivery,
      RaioAtendimento: loja.RaioAtendimento,
      DadosBancario: loja.DadosBancario,
      LojaTipoLoja: loja.LojaTipoLoja,
      FotoFachada: loja.FotoFachada,
      TerminaisLoja: loja.TerminaisLoja,
      ProdutoAtivacaoLoja: loja.ProdutoAtivacaoLoja,
      CodigoInfox: loja.CodigoInfox
    };

    if (loja.Endereco !== null) {
      obj.Endereco = loja.Endereco;
    }

    return this.defaultHttp.request("post", 'api/loja/salvarLoja/'+ HttpProvider.userAuth.PessoaId, obj);
  }

  salvarLojaIndicacao(loja) {

    var obj = {
      Id: loja.Id,
      Codigo: loja.Codigo,
      Descricao: loja.Descricao,
      Cnpj: loja.Cnpj,
      RazaoSocial: loja.RazaoSocial,
      Telefone: loja.Telefone,
      Celular: loja.Celular,
      Endereco: {}
    };

    if (loja.Endereco !== null) {
      obj.Endereco = loja.Endereco;
    }

    return this.defaultHttp.request("post", 'api/loja/salvarLojaIndicacao/', obj);
  }

  atualizarLoja(loja){
    return this.defaultHttp.request("post", 'api/loja/salvarLoja/'+ HttpProvider.userAuth.PessoaId, loja);
  }

  salvarLogoLoja(idLoja, logo){

    var obj = {
      IdLoja: idLoja,
      ImageData: logo
    }

    return this.defaultHttp.request("post", 'api/loja/salvarLogoLoja/'+ HttpProvider.userAuth.PessoaId, obj);
  }

  getTutorial() {
    return this.defaultHttp.cached_request('api/tutorial/tutoriais/');
  }

  getBannersHome() {
    return this.defaultHttp.cached_request('api/banner/home/');
  }

  changeLojaAtiva(novaLoja: number) {
    return this.defaultHttp.request('post',`api/listacompra/mudarLojaAtiva/${novaLoja}/${HttpProvider.userAuth.UsuarioId}`);
  }

  salvarProdutoPrecoLoja(idLoja, produto) {
    return this.defaultHttp.request("post", 'api/loja/salvarProdutoPrecoLoja/'+ idLoja, produto);
  }

  getLojasPorNomeDocumentoOuCidade(estado, cidade, bairro, query, lote: number = 2, page: number = 0, quantidade: number = 0) {
    let lojas = this.defaultHttp.request("get", `api/loja/LojasPorNomeDocumentoOuCidade?estado=${estado}&cidade=${cidade}&bairro=${bairro}&dadosPesquisa=${query}&lote=${lote}&inicio=${page}&quantidade=${quantidade}`);
    return lojas;
  }

  public getLojasPorNome(nome, page: number = 0, quantidade: number = 0) {
    let lojas = this.defaultHttp.request("get", `api/loja/LojasPorNome?nome=${nome}&inicio=${page}&quantidade=${quantidade}`);
    return lojas;
  }

  public getTipoEstabelecimentoPorNome(nome) {
    return this.defaultHttp.request("get", `api/loja/TipoEstabelecimentoPorNome?nome=${nome}`);
  }
}
