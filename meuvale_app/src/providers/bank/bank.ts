import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';

@Injectable()
export class BankProvider {  

  constructor(public http: HttpProvider) {}

  loadBanks(): Observable<any> {
    return this.http.cached_request('api/banco/bancos/');
  }
}
