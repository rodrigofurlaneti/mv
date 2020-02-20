import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Storage } from '@ionic/storage';
import { Observable } from 'rxjs/Observable';
import { API_CONFIG } from '../../app/config/api.config';
import { MyApp } from '../../app/app.component';
import { SystemKeysConfig } from '../../app/config/system-keys.config';
import { AuthToken } from '../auth/auth';

@Injectable()
export class HttpProvider {
  public static userAuth: AuthToken;

  constructor(private http: HttpClient, private storage: Storage) {}

  /**
   * request com cachê
   * 
   * @param _method método http: get, post, put e etc.
   * @param _path Caminho relativo contido no endpoint principal
   * @param _data objeto a ser enviado ao servidor
   */
  public cached_request(_path: string): Observable<any> {
    return Observable.create(
      observer => {
        this.storage.get(_path)
          .then(
            data => {
              if(data)
                observer.next(data);

              this.request("get", _path)
                .subscribe(
                  data => {
                    this.storage.set(_path, data); 
                    observer.next(data)
                  },
                  error => observer.error(error),
                  () => observer.complete()
                );

            }
          )
      }
    ); 
  }

  /**
   * 
   * @param _method método http: get, post, put e etc.
   * @param _path Caminho relativo contido no endpoint principal
   * @param _data objeto a ser enviado ao servidor
   */
  public request(_method: string, _path: string, _data?: any, _options?: any): Observable<any> {
    const _fullPath: string = `${API_CONFIG.apiUrl}${_path}`;
    return Observable.create(observer => {
      this.doOriginalRequest(_method, _fullPath, _data, observer, true, _options);
    });
    
  }

  /**
   * Efetua a chamada original
   * 
   * @param _method método http: get, post, put e etc.
   * @param _fullPath caminho completo do serviço
   * @param _data objeto a ser enviado ao servidor
   * @param observer observer delegado.
   * @param _retry erro gerado na chamada original.
   */
  private doOriginalRequest(_method: string, _fullPath: string, _data:any, observer: any, _retry: boolean = true, _options?: any) {
    this.createHeader(_method, _data, _options != null ?_options.headers : null)
        .subscribe(
          options => {
            if(_options && _options.headers)
              delete _options.headers;
            options = {...options, ..._options};  
            this.http.request(_method, _fullPath, options)
              .subscribe(
                res => {observer.next(res); console.log("rs") },
                error => { this.handleError(_method, _fullPath, _data, observer, error, _retry) },
                () => {observer.complete(); console.log("complete")}
              )
          }, error => {
            console.log("error")
            observer.error(error);
          }, () => {
            console.log("complete")
            observer.complete();
          }
        );
  }

  /**
   * Tratamento de erro, caso o erro seja 401, faz uma nova tentativa após gerar um novo token.
   * 
   * @param _method método http: get, post, put e etc.
   * @param _fullPath caminho completo do serviço
   * @param _data objeto a ser enviado ao servidor
   * @param observer observer delegado.
   * @param error erro gerado na chamada original.
   */
  private handleError(_method: string, _fullPath: string, _data: any, observer: any, error: any, retry: boolean): void {
    if(error.status == 401) {
      if(retry) {
        this.storage.get(SystemKeysConfig.LOGIN_SECURE)
            .then(
              data => {
                const loginSecure = data;
                console.log(loginSecure);
                if(loginSecure.loginType == SystemKeysConfig.USER_PASS) {
                  this.authenticate(loginSecure)
                    .subscribe(
                      data => this.doOriginalRequest(_method, _fullPath, _data, observer, false),
                      error => observer.error(error)
                    );
                }else if(loginSecure.loginType == SystemKeysConfig.USER_FACEBOOK) {
                  this.authenticateFacebook(loginSecure)
                    .subscribe(
                      data => this.doOriginalRequest(_method, _fullPath, _data, observer, false),
                      error => observer.error(error)
                    );
                }else observer.error(error);
    
              }, error =>{
                console.log(error);
                observer.error(error);
              }
            ).catch(error => {
              observer.error(error);
            })
          .catch(error => observer.error(error));
      } else {
        MyApp.ref.logout();
        observer.error(error);
      }
    } else {
      observer.error(error);
    }
  }

  /**
   * Faz a montagem do header com o token e com o tipo de cabeçalho
   * 
   * @param _method método http: get, post, put e etc.
   * @param _data objeto a ser enviado ao servidor
   */
  private createHeader(_method: string, _data: any, _h?: HttpHeaders): Observable<any> {
    return Observable.create(observer => {
      this.storage.get(SystemKeysConfig.CLAIMS)
        .then(data => {
          let defaultHeaders = {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + (data ? data.access_token : "")
          };

          if(_h != null){
            _h.keys().map((key) => { 
              defaultHeaders[key] = _h.get(key);
             });
          }

          let h = new HttpHeaders(defaultHeaders);

          console.log(h)
          const _options : any= {
            body: _data,
            method: _method,
            headers: h
          }
          observer.next(_options);
        }).catch(error => {
          observer.error(error);
        })
    });
  }

  /**
   * Autencicaçao do usuário simples.
   * 
   * @param usercreds dados de login
   */
  private authenticate(usercreds: any): Observable<any> {
    return Observable.create(
      observer => {
        // TODO: MOCK PARA TESTE - REMOVER --------------------
        //usercreds.name = 'leandrogrando@leojarts.com.br';
        //usercreds.password = '123';
        //let isPersistent = false; //é o lembrar-me
        //----------------------------------------------

        let headers = new HttpHeaders
        headers.append('Content-Type', 'application/x-www-form-urlencoded');
        let data = 'grant_type=password&username=' + usercreds.name + '&password=' + usercreds.password + '&IsPersistent=' + false;
        
        this.http.post(`${API_CONFIG.apiUrl}token`, data, { headers: headers })
          .subscribe((data: AuthToken) => {
            HttpProvider.userAuth = data;
            this.storage.set(SystemKeysConfig.CLAIMS, data).then(() => {});
            this.setSecureStorage(usercreds, SystemKeysConfig.USER_PASS, observer);
          }, error => {
            observer.error(error);
          });
      }
    );
}

/**
 * Autenticação via facebook
 * 
 * @param usercreds dados de login
 */
private authenticateFacebook(usercreds: any): Observable<any> {
  return Observable.create(
    observer => {
      // TODO: MOCK PARA TESTE - REMOVER --------------------
      //usercreds.name = 'leandrogrando@leojarts.com.br';
      //usercreds.password = '123';
//      let isPersistent = false; //é o lembrar-me
      //----------------------------------------------

      let headers = new HttpHeaders();
      headers.append('Content-Type', 'application/x-www-form-urlencoded');
      let data = 'email=' + usercreds.email + '&facebookid=' + usercreds.facebookId;
      this.http.post(`${API_CONFIG.apiUrl}api/usuario/facebookLogin`, data, { headers: headers })
        .subscribe((data: AuthToken) => {
          HttpProvider.userAuth = data;
          this.storage.set(SystemKeysConfig.CLAIMS, data).then(() => {});
          this.setSecureStorage(usercreds, SystemKeysConfig.CLAIMS, observer);          
        }, error => {
          observer.error(error);
        });
    });
  }

  private setSecureStorage(usercreds, type, observer) {
    this.storage.set(SystemKeysConfig.LOGIN_SECURE,
        {
          loginType: type,
          email: usercreds.email,
          name: usercreds.name,
          password: usercreds.password,
          facebookId: usercreds.facebookId
        }
      ).then( res => observer.next(res) )
      .catch(error => observer.error(error));
  }
}