import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';

@Injectable()
export class FornecedorProvider {

  constructor(private defaultHttp: HttpProvider) {}

  public list():Observable<any> {
    return this.defaultHttp.request("get", 'api/fornecedor/fornecedores');
  }

  fornecedores(lat, lng, inicio = 0, quantidade = 10) {
    let lojas = this.defaultHttp.request("get", 'api/fornecedor/fornecedoresProximos?latitude=' + lat + '&longitude=' + lng+ '&inicio=' + inicio + '&quantidade=' + quantidade);
    return lojas;
  }

  fornecedoresPorClassificacao(lat, lng, inicio = 0, quantidade = 10, classificacao: string) {
    let lojas = this.defaultHttp.request("get", 'api/fornecedor/fornecedoresProximosPorClassificacao?latitude=' + lat + '&longitude=' + lng+ '&inicio=' + inicio + '&quantidade=' + quantidade + '&classificacao=' + classificacao);
    return lojas;
  }

  public listProducts(fornecedorId: number):Observable<any> {
    return this.defaultHttp.request("get", 'api/fornecedor/produtos/' + fornecedorId);
  }

  public listProductsByUser(fornecedorId: number, usuarioId: number):Observable<any> {
    return this.defaultHttp.request("get", 'api/fornecedor/produtos/' + fornecedorId + '/' +  usuarioId);
  }

  public getByBarrCode(lojaId: number, codBarras: string, usuarioId: number):Observable<any> {
    return this.defaultHttp.request("get", 'api/fornecedor/produtoCodBarras/' + lojaId + '/' + codBarras + '/' + usuarioId);
  }

  public getById(id: number): Observable<any> {
    return this.defaultHttp.request("get", 'api/fornecedor/' + id);
  }

  public getProdutoPorCodigoFornecedor(fornecedorId, codigo) {
    return this.defaultHttp.request("get", 'api/fornecedor/produtoCodigo/' + fornecedorId + '/' + codigo );
  }

  public salvarProdutoPrecoFornecedor(idFornecedor, produto) {
    return this.defaultHttp.request("post", 'api/fornecedor/salvarProdutoPrecoFornecedor/'+ idFornecedor, produto);
  }
}
