import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';

@Injectable()
export class PeopleProvider {

  constructor(private http: HttpProvider) {}

  registerDevice(useriD: string, deviceId: string) {
    this.http.request("post", `api/dispositivo/adicionar/${useriD}`, deviceId)
      .subscribe(data => console.log("device registerd"),
        error => console.log(error));
  }

  getPessoa(id):Observable<any> {
    return this.http.cached_request('api/pessoa/' + id);
  }

  getPaises():Observable<any> {
      return this.http.cached_request('api/pais/');
  }

  getEstados():Observable<any> {
      return this.http.cached_request('api/Estado/');
  }

  getCidades(estadoId):Observable<any> {
      return this.http.cached_request('api/Cidade/getCidadesByEstados/' + estadoId);
  }

  public save(usuario: any): Observable<any> {
    return this.http.request("post", 'api/usuario/account/register', usuario);
  }

  public alterpassword(usuario: any): Observable<any> {
    return this.http.request("post", 'api/usuario/account/alterpassword', usuario);
  }
  
  public update(pessoa): Observable<any> {
    
    return this.http.request("post", `api/pessoa/update/${pessoa.Pessoa.Id}`, pessoa.Pessoa);
  }

  public saveAddress(address: any): Observable<any> {
    return this.http.request("post", 'api/endereco', address)
  }

  public removeAddress(address: any): Observable<any> {
    return this.http.request("post", 'api/endereco/deletar/' + address.Id)
  }

  public updateAddress(address: any): Observable<any> {
    return this.http.request("post", 'api/endereco/' + address.Pessoa.Id, address)
  }

  public listExclusiveDiscounts(id: number): Observable<any> {
    return this.http.request("post", 'api/pessoa/descontosexclusivos/' + id)
  }

  public listDiscounts(id: number): Observable<any> {
    return this.http.request("post", 'api/pessoa/descontos/' + id)
  }

  public saveContact(dta: any): Observable<any> {
    return this.http.request("post", 'api/usuario/formularioContato', dta);
  }

}
