import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';

@Injectable()
export class PaymentProvider {  

  constructor(public http: HttpProvider) {}

  public save(card: any): Observable<any> {
    return this.http.request("post", 'api/cartao', card);
  }

  public update(card: any): Observable<any> {
    return this.http.request("post", 'api/cartao/' + card.Id, card);
  }
  
  public remove(card: any): Observable<any> {
    return this.http.request("post", 'api/cartao/deletar/' + card.Id);
  }
  
  loadCartoes(): Observable<any> {
    return this.http.cached_request('api/cartao/pessoa/' + HttpProvider.userAuth.PessoaId);
  }

  public saldo(card: any): Observable<any> {
    return this.http.request("post", `api/cartao/saldo/${HttpProvider.userAuth.PessoaId}/${card.NumeroSemMascara}/${card.Senha}`);
  }
  public extrato(card: any, estabelecimentoId): Observable<any> {
    return this.http.request("post", `api/cartao/extrato/${HttpProvider.userAuth.PessoaId}/${card.NumeroSemMascara}/${card.Senha}/${estabelecimentoId}`);
  }
}
