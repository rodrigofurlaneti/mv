import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';

@Injectable()
export class ConvenioProvider {

  constructor(public defaultHttp: HttpProvider) {

  }

  getById(codigo) {
    return this.defaultHttp.request("get", `api/convenio/${codigo}`);
  }

  getAll() {
    return this.defaultHttp.request("get", `api/convenio/`);
  }

  save(item) {
    return this.defaultHttp.request("post", 'api/convenio/', item);
  }

  getPorNomeDocumentoOuCidade(estado, cidade, bairro, query, page: number = 0, quantidade: number = 10) {
    let lojas = this.defaultHttp.request("get", `api/convenio/PorNomeDocumentoOuCidade?estado=${estado}&cidade=${cidade}&bairro=${bairro}&dadosPesquisa=${query}&inicio=${page}&quantidade=${quantidade}`);
    return lojas;
  }

  delete(item) {
    return this.defaultHttp.request("delete", `api/convenio/${item.Id}`);
  }
}
