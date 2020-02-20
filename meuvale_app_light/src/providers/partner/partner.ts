import { Injectable, EventEmitter } from '@angular/core';
import { HttpProvider } from '../http/http';

@Injectable()
export class PartnerProvider {
  private static _PartnerSelected: any;
  public static readonly PartnerChange: EventEmitter<any> = new EventEmitter();
  
  constructor(private defaultHttp: HttpProvider) {}
  
  public static getCurrentPartner(): any {
    return PartnerProvider._PartnerSelected;
  }

  public static setCurrentPartner(fornecedor: any): any {
    PartnerProvider._PartnerSelected = fornecedor;
    PartnerProvider.PartnerChange.emit(fornecedor);
  }

  fornecedors(lat, lng) {
    return this.defaultHttp.request("get", 'api/fornecedor/fornecedorsProximas?latitude=' + lat + '&longitude=' + lng);
  }

  fornecedores() {
    return this.defaultHttp.cached_request('api/fornecedor/fornecedores')
  }

  fornecedorsUsuario(pessoaId, lat, lng) {
    return this.defaultHttp.request("get", 'api/fornecedor/fornecedorsUsuario?pessoa=' + pessoaId + '&latitude=' + lat + '&longitude=' + lng);
  }

  getPrecoPorfornecedorProduto(fornecedorId, produtoId) {
      let params = { fornecedorId: fornecedorId, produtoId: produtoId };
      return this.defaultHttp.request("get", 'api/preco', params);
  }

  getProdutos(fornecedorId, page: number = 0) {
    return this.defaultHttp.request("get", `api/fornecedor/${fornecedorId}/produtos?inicio=${page}&quantidade=10`);
  }
  
  getProdutosFornecedor(fornecedorId) {
    return this.defaultHttp.request("get", 'api/fornecedor/' + fornecedorId);
  }

  getProdutosFornecedorPorUsuario(fornecedorId, usuarioId) {
    return this.defaultHttp.request("get", 'api/fornecedor/produtos/' + fornecedorId + '/' +  usuarioId)
  }

  getProdutoPorCodBarras(fornecedorId, codBarras) {
      return this.defaultHttp.request("get", 'api/fornecedor/' + fornecedorId + '/produtoCodBarras?codBarras=' + codBarras);
  }

  getProdutoPorCodBarrasFornecedor(fornecedorId, codBarras, usuarioId) {
    return this.defaultHttp.request("get", 'api/fornecedor/produtoCodBarras/' + fornecedorId + '/' + codBarras + '/' + usuarioId);
  }

  getfornecedor(id) {
    return this.defaultHttp.request("get", 'api/fornecedor/' + id);
  }

  getFornecedor(id) {
    return this.defaultHttp.request("get", 'api/fornecedor/' + id);
  }

  getTiposClassif() {
    return this.defaultHttp.request("get", 'api/fornecedor/tiposClassificacao');
  }

  getClassificacoes(tipo) {
    return this.defaultHttp.request("get", 'api/fornecedor/classificacoes/' + tipo);
  }

  salvarfornecedor(fornecedor) {

    var obj = {
      Id: fornecedor.Id,
      Codigo: fornecedor.Codigo,
      Descricao: fornecedor.Descricao,
      Cnpj: fornecedor.Cnpj,
      RazaoSocial: fornecedor.RazaoSocial,
      InscricaoEstadual: fornecedor.InscricaoEstadual,
      Classificacao: fornecedor.Classificacao,
      Telefone: fornecedor.Telefone,
      Celular: fornecedor.Celular,
      Email: fornecedor.Email,
      fornecedorAprovada: fornecedor.fornecedorAprovada,
      Status: fornecedor.Status,
      ImagemUpload: fornecedor.ImagemUpload,
      Endereco: {}
    };

    if (fornecedor.Endereco !== null) {
      obj.Endereco = fornecedor.Endereco;
    }

    return this.defaultHttp.request("post", 'api/fornecedor/salvarfornecedor/'+ HttpProvider.userAuth.PessoaId, obj);
  }

  atualizarfornecedor(fornecedor){
    return this.defaultHttp.request("post", 'api/fornecedor/salvarfornecedor/'+ HttpProvider.userAuth.PessoaId, fornecedor);
  }

  salvarLogofornecedor(idfornecedor, logo){

    var obj = {
      Idfornecedor: idfornecedor,
      ImageData: logo
    }

    return this.defaultHttp.request("post", 'api/fornecedor/salvarLogofornecedor/'+ HttpProvider.userAuth.PessoaId, obj);
  }

  getTutorial() {
    return this.defaultHttp.cached_request('api/tutorial/tutoriais/');
  }

  getBannersHome() {
    return this.defaultHttp.cached_request('api/banner/home/');
  }

  changefornecedorAtiva(novafornecedor: number) {
    return this.defaultHttp.request('post',`api/listacompra/mudarfornecedorAtiva/${novafornecedor}/${HttpProvider.userAuth.UsuarioId}`);
  }

  salvarProdutoPrecofornecedor(idfornecedor, produto) {
    return this.defaultHttp.request("post", 'api/fornecedor/salvarProdutoPrecofornecedor/'+ idfornecedor, produto);
  }
}
