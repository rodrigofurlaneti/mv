import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';
import { Storage } from '@ionic/storage';
import { API_CONFIG } from '../../app/config/api.config';
import { Observable } from 'rxjs/Observable';
import { SystemKeysConfig } from '../../app/config/system-keys.config';
import { HttpProvider } from '../http/http';

@Injectable()
export class AuthProvider {

  private defaultHeader: HttpHeaders;
  private static _eventDispatcher: EventEmitter<AuthToken> = new EventEmitter<AuthToken>();

  constructor(public http: HttpClient,
    private storage: Storage) {
    this.defaultHeader = new HttpHeaders();
    this.defaultHeader.append('Content-Type', 'application/x-www-form-urlencoded');
  }

  public static get eventDispatcher(): EventEmitter<AuthToken> {
    return AuthProvider._eventDispatcher;
  }

  public getUserLogged(): Observable<AuthToken> {
    return Observable.create(
      observer => {
        this.storage.get(SystemKeysConfig.CLAIMS)
          .then(data => {
            HttpProvider.userAuth = data;
            observer.next(data);
          })
          .catch(error => observer.next(null));
      }
    )
  }

  public authenticate(usercreds: { name: string, password: string }): Observable<AuthToken> {

    const isPersistent = false; //é o lembrar-me    
    const data = 'grant_type=password&username=' + usercreds.name + '&password=' + usercreds.password + '&IsPersistent=' + isPersistent;

    console.log(data);

    return Observable.create(observer => {
      this._doAuthenticate(SystemKeysConfig.USER_PASS, "token", usercreds, data, observer);
    });
  }

  private _doAuthenticate(typeLogin: string, path: string, credentials: any, data: any, observer: any): void {
    this.http.post<AuthToken>(`${API_CONFIG.apiUrl + path}`, data, { headers: this.defaultHeader })
      .subscribe(
        res => {
          console.log("resultado do token")
          console.log(res);
          HttpProvider.userAuth = res;
          this.storage.set(SystemKeysConfig.CLAIMS, res)
          this.setSecureStorage(credentials, typeLogin);
          observer.next(res);
          AuthProvider._eventDispatcher.emit(res);
        },
        error => { observer.error(error) },
        () => observer.complete()
      )
  }

  private setSecureStorage(usercreds, type) {
    this.storage.set(SystemKeysConfig.LOGIN_SECURE,
      {
        loginType: type,
        email: usercreds.email,
        name: usercreds.name,
        password: usercreds.password
      })
      .then(res => console.log(res))
      .catch(error => console.log(error));
  }

  rememberPassword(email) {
    const data = "email=" + email;
    return this.http.post(`${API_CONFIG.apiUrl}api/usuario/forgotpassword`, data, { headers: this.defaultHeader });

  }

}

export interface AuthToken {
  PerfilId: string;
  PessoaId: string;
  UsuarioId: string;
  UsuarioNome: string;
  access_token: string;
  expires_in: number;
  token_type: string;
  PrimeiroLogin: string;
}