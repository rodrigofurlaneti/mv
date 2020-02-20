
import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';
import { ProductItem } from '../../model/product-item';
import { map } from 'rxjs/operators';
import { StoreProvider } from '../store/store';
import { ProductLoja } from '../../model/product-loja';
import { ShoppingProvider } from '../shopping/shopping';

@Injectable()
export class ProductProvider {

  public max_page_size: number = 10;
  constructor(public defaultHttp: HttpProvider) {

  }

  private get currentStorage(): string {
    return StoreProvider.getCurrentStore().Id;
  }

  public listSecoes(): Observable<any> {
    return this.defaultHttp.cached_request('api/secaoProduto/secoes');
  }
  public listSecoesPorDepartamento(departamento: number): Observable<any> {
    return this.defaultHttp.cached_request(`api/secaoProduto/secoesPorDepartamento/${departamento}`);
  }

  public listCategorias(): Observable<any[]> {
    return this.defaultHttp.cached_request('api/categoriaProduto/categorias');
  }

  public listDepartamentos(): Observable<any[]> {
    return this.defaultHttp.cached_request(`api/departamentoProduto/departamentos`);
  }

  public listDepartamentosPorLoja(loja: number): Observable<any[]> {
    return this.defaultHttp.cached_request(`api/departamentoProduto/departamentosPorLoja/${loja}`);
  }

  public listDepartamentosPorCategoria(categoria: number): Observable<any[]> {
    return this.defaultHttp.cached_request(`api/departamentoProduto/departamentosPorCategoria/${categoria}`);
  }

  public listProdutoPorDepartamento(departamento: number, page: number = 1): Observable<ProductLoja[]> {
    return this.defaultHttp.cached_request(`api/produto/produtosPorDepartamento/${departamento}/${this.currentStorage}?inicio=${page}&quantidade=${this.max_page_size}`)
      .pipe(map(data => ProductLoja.listFromData(data)));
  }

  public getProdutosFornecedor(forncedorId: number, page: number = 0) {
    return this.defaultHttp.request("get", `api/fornecedor/${forncedorId}/produtos?inicio=${page}&quantidade=10`)
      .pipe(map(data => ProductLoja.listFromData(data)));
  }

  public getProdutosLoja(forncedorId: number, page: number = 0) {
    return this.defaultHttp.request("get", `api/loja/${forncedorId}/produtos?inicio=${page}&quantidade=10`)
      .pipe(map(data => ProductLoja.listFromData(data)));
  }

  public adicionarNovamente(idListaAntiga: number) {
    return this.defaultHttp.request('post', `/api/listacompra/recuperarESubstituirItens/${ShoppingProvider.getCurrentLista().Id}/${idListaAntiga}`);
  }

  getProdutoPorCodBarras(codBarras) {
    return this.defaultHttp.request("get", `api/produto/produtoPorCodigoBarras/${codBarras}`);
  }

  getProdutoPorCodigo(codigo) {
    return this.defaultHttp.request("get", `api/produto/produtoPorId/${codigo}`)
    .pipe(map(data => new ProductLoja(data)));;
  }

  setCategoria(categoria: any) {
    return this.defaultHttp.request('post', 'api/categoriaProduto/', categoria);
  }

  setDepartamento(departamento: any) {
    return this.defaultHttp.request('post', 'api/DepartamentoProduto/', departamento);
  }

  setSecao(secao: any) {
    return this.defaultHttp.request('post', 'api/secaoProduto/', secao);
  }

  setGrupo(grupo: any) {
    return this.defaultHttp.request('post', 'api/grupoProduto/', grupo);
  }

  setSubGrupo(subGrupo: any) {
    return this.defaultHttp.request('post', 'api/subGrupoProduto/', subGrupo);
  }

  getOfertas(page: number = 0, classificacao: string = "") {
    return this.defaultHttp.request("get", `api/produto/ofertas?inicio=${page}&quantidade=10&classificacao=${classificacao}`)
      .pipe(map(data => ProductLoja.listFromData(data)));
  }

  public getProdutos(page: number = 0) {
    return this.defaultHttp.request("get", `api/produto/produtos?inicio=${page}&quantidade=10`)
      .pipe(map(data => ProductLoja.listFromData(data)));
  }

  public getProdutosPorNome(nome, page: number = 0) {
    return this.defaultHttp.request("get", `api/produto/produtosPorNome?nome=${nome}&inicio=${page}&quantidade=10`)
      .pipe(map(data => ProductLoja.listFromData(data)));
  }

  public getDepartamentosPorNome(nome) {
    return this.defaultHttp.request("get", `api/DepartamentoProduto/DepartamentosPorNome?nome=${nome}`);
  }
}
