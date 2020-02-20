import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';
import { StoreProvider } from '../store/store';

@Injectable()
export class ShoppingProvider {

  private static listaAtiva: any;


  constructor(public defaultHttp: HttpProvider) { }

  public static getCurrentLista(): any {
    return ShoppingProvider.listaAtiva;
  }
  public static setCurrentLista(lista: any): any {
    ShoppingProvider.listaAtiva = lista;
  }

  trocarLoja(Id: any): Observable<any> {
    return Observable.create(observer => {
      this.defaultHttp.request("post", `api/listacompra/mudarLojaAtiva/${ShoppingProvider.listaAtiva.Id}/${Id}`)
        .subscribe(
          data => {
            this.getListaAtiva()
              .subscribe(
                () => {
                  observer.next(data);
                  observer.complete();
                }, error => observer.error(error)
              );

          }, error => observer.error(error)
        )
    })



  }

  getListaTemporaria(lojaId): Observable<any> {
    return Observable.create(
      observer => {

        if (ShoppingProvider.listaAtiva && ShoppingProvider.listaAtiva.Loja != null) {
          if (ShoppingProvider.listaAtiva.Loja.Id == lojaId) {
            observer.next(ShoppingProvider.listaAtiva);
            observer.complete();
            StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
            return;
          }
        }
        const claims = HttpProvider.userAuth;
        let lista = {
          total: 0,
          pedido: null,
          usuario: null,
          loja: { Id: lojaId },
          itens: []
        };

        if (claims)
          lista.usuario = { Id: claims.UsuarioId };

        this.defaultHttp.request("post", 'api/listacompra/', lista)
          .subscribe(data => {
            ShoppingProvider.listaAtiva = data;
            StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
            observer.next(data);
            observer.complete();
          }, error => {
            console.log(error);
            observer.next(error);
          });
      }
    )


  }

  getListaTemporariaFornecedor() {
    if (ShoppingProvider.listaAtiva) {
      return;
    }

    const claims = HttpProvider.userAuth;

    let lista = {
      total: 0,
      pedido: null,
      usuario: null,
      loja: null,
      itens: []
    };

    if (claims)
      lista.usuario = { Id: claims.UsuarioId };

    this.defaultHttp.request("post", 'api/listacompra/', lista)
      .subscribe(data => {
        ShoppingProvider.listaAtiva = data;
        StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
      }, error => {
        console.log(error);
      });
  }

  getListaAtiva(): Observable<any> {

    const claims = HttpProvider.userAuth;

    if (claims) {
      if (ShoppingProvider.listaAtiva)
        if (ShoppingProvider.listaAtiva.Usuario.Id == 0) {
          ShoppingProvider.listaAtiva.Usuario = { Id: claims.UsuarioId };
        }

      return Observable.create(observer => {
        this.defaultHttp.request("get", `api/listacompra/usuario/${claims.UsuarioId}`)
          .subscribe(data => {
            let lista = data;
            if (lista) {
              ShoppingProvider.listaAtiva = lista;
              StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
              observer.next(lista);
            } else {
              observer.next(null);
            }
          }, error => {
            observer.error(error);
          }, () => observer.complete());
      });
    }
    else
      return Observable.create(obsersver => {
        obsersver.next(ShoppingProvider.listaAtiva);
      })
  }

  getListaAtivaLoja(loja): Observable<any> {

    const claims = HttpProvider.userAuth;

    if (claims) {
      return Observable.create(observer => {
        this.defaultHttp.request("get", `api/listacompra/usuario/${claims.UsuarioId}/${loja.Id}`)
          .subscribe(data => {
            let lista = data;
            if (lista && lista.Id > 0) {
              ShoppingProvider.listaAtiva = lista;
              StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
              observer.next(lista);
            } else {
              this.getListaTemporaria(loja.Id)
                .subscribe(
                  data => {
                    lista = data;
                    observer.next(lista);
                  },
                  error => observer.next(null)
                )
            }
          }, error => {
            observer.error(error);
          }, () => observer.complete());
      });
    }
    else
      return Observable.create(obsersver => {
        obsersver.next(ShoppingProvider.listaAtiva);
      })
  }

  getListaAtualizada(): Observable<any> {

    return Observable.create(observer => {
      this.defaultHttp.request("get", `api/listacompra/${ShoppingProvider.listaAtiva.Id}`)
        .subscribe(data => {
          let lista = data;
          if (lista != null) {
            ShoppingProvider.listaAtiva = lista;
            StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
            observer.next(ShoppingProvider.listaAtiva);
          } else {

            observer.next(null);
          }
        }, error => {
          observer.error(error);
        }, () => observer.complete());
    });
  }

  atualizarLista(): Observable<any> {

    return Observable.create(observer => {
      this.defaultHttp.request("get", `api/listacompra/${ShoppingProvider.listaAtiva.Id}/atualizar`)
        .subscribe(data => {
          let lista = data;
          if (lista != null) {
            ShoppingProvider.listaAtiva = lista;
            StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);

            observer.next(ShoppingProvider.listaAtiva);
          } else {
            observer.next(null);
          }
        }, error => {
          observer.error(error);
        }, () => observer.complete());
    });
  }

  adicionarItem(item): Observable<any> {
    return Observable.create(observer => {
      if (!item || !item.Quantidade || isNaN(item.Quantidade) || item.Quantidade < 1) {
        observer.error('Por favor informe uma quantidade válida.')
      }
      if (!ShoppingProvider.listaAtiva) {
        observer.error('Não há lista ativa.');
      }

      this.defaultHttp.request("post", `api/listacompra/${ShoppingProvider.listaAtiva.Id}/adicionaritem`, item)
        .subscribe(data => {
          ShoppingProvider.listaAtiva = data
          StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
          observer.next("Item adicionado à lista de compras.");
        }, error => {
          observer.error(error);
        }, (complete => {
          observer.complete();
        }) as any);
    });

  }

  alterarItem(item) {
    return Observable.create(observer => {
      if (!item || !item.Quantidade || isNaN(item.Quantidade) || item.Quantidade < 1) {
        observer.error('Por favor informe uma quantidade válida.');
      }

      this.defaultHttp.request("post", `api/listacompra/${ShoppingProvider.listaAtiva.Id}/alteraritem`, item)
        .subscribe(data => {
          ShoppingProvider.listaAtiva = data;
          StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
          observer.next(data);
        }, error => {
          observer.error(error.text());
        }, (complete => {
          observer.complete;
        }) as any);
    });
  }

  removerItem(item) {
    return Observable.create(observer => {
      this.defaultHttp.request("post", `api/listacompra/${ShoppingProvider.listaAtiva.Id}/removeritem`, item)
        .subscribe(data => {
          ShoppingProvider.listaAtiva = data;
          StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
          observer.next(data);
        }, error => {
          observer.error(error.text());
        }, (complete => {
          observer.complete();
        }) as any);
    });
  }

  removerListaAtual(): Observable<any> {
    return Observable.create(
      observer => {
        this.defaultHttp.request('post', `api/listacompra/${ShoppingProvider.listaAtiva.Id}/removerTodosItens`)
          .subscribe(
            data => {
              ShoppingProvider.listaAtiva = data;
              observer.next(data);
            }, error => observer.error(error),
            () => observer.complete()
          )
      }
    )
  }

  validaCupom(cupom, listaCompraAtiva) {

    cupom = cupom === "" || cupom == null ? "null" : cupom;

    return Observable.create(observer => {
      this.defaultHttp.request("post", `api/listacompra/validaCupom/${cupom}/${listaCompraAtiva.Id}`)
        .subscribe(data => {
          if (data.Mensagem !== "") {
            console.log("Tipo [" + data.TipoMensagem + "] Validar Cupom: " + data.Mensagem);
          }
          ShoppingProvider.listaAtiva = data.ObjetoRetorno;
          StoreProvider.setCurrentStore(ShoppingProvider.listaAtiva.Loja);
          observer.next(data.ObjetoRetorno);
        }, error => {
          observer.error(error);
        }, (complete => {
          observer.complete();
        }) as any);
    });


  }

  getDescontos() {
    return this.defaultHttp.cached_request('api/desconto/todosdescontos/');
  }

  getTabloide() {

    return this.defaultHttp.cached_request('api/tabloide/tabloideatual/');

  }

}
