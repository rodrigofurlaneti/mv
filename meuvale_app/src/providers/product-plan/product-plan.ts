import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';

@Injectable()
export class ProductPlanProvider {

  constructor(public defaultHttp: HttpProvider) {

  }

  public getPorConvenio(convenioId: number, page: number = 0, quantidade = 10) {
    return this.defaultHttp.request("get", `api/convenio/${convenioId}/planos?inicio=${page}&quantidade=${quantidade}`);
  }

  public getPlanosPorUsuario() {
    return this.defaultHttp.request("get", `api/convenio/planosPorUsuario`);
  }

  getPorCodigo(codigo) {
    return this.defaultHttp.request("get", `api/planovenda/${codigo}`);
  }

  getAll() {
    return this.defaultHttp.request("get", `api/planovenda/`);
  }

  save(plano) {
    return this.defaultHttp.request("post", 'api/planovenda/', plano);
  }

  delete(plano) {
    return this.defaultHttp.request("delete", `api/planovenda/${plano.Id}`);
  }
}
