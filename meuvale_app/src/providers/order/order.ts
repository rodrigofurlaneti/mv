import { Injectable } from '@angular/core';
import { HttpProvider } from '../http/http';
import { Observable } from 'rxjs';
import { UtilsProvider } from '../../providers/utils/utils';

/*
  Generated class for the OrderProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
@Injectable()
export class OrderProvider {

  constructor(private http: HttpProvider,
    private utils: UtilsProvider) { }

  public finalize(pedido: any): Observable<any> {
    var pedidoTransf = this.montaDadosPedido(pedido);
    return this.http.request("post", 'api/pedido/finalizar', pedidoTransf);
  }

  public finalizePlanoVenda(pedido: any): Observable<any> {
    var pedidoTransf = this.montaDadosPedidoPlanoVenda(pedido);
    return this.http.request("post", 'api/pedido/finalizar', pedidoTransf);
  }

  public finalizeVoucher(pedido: any): Observable<any> {
    var pedidoTransf = this.montaDadosPedidoVoucher(pedido);
    return this.http.request("post", 'api/pedido/finalizarVoucher', pedidoTransf);
  }

  public saveRateOfOrder(pedidoId: number, avaliacaoPedido: any): Observable<any> {
    return this.http.request("post", 'api/pedido/salvarAvaliacaoPedido/' + pedidoId, avaliacaoPedido);
  }

  public list(): Observable<any> {
    return this.http.request("get", 'api/pedido/');
  }

  public listByStore(idLoja, inicio, quantidade): Observable<any> {
    return this.http.request("get", `api/pedido/pedidoLoja/${idLoja}/${inicio}/${quantidade}`);
  }

  public get(id: number): Observable<any> {
    return this.http.request("get", `api/pedido/${id}`);
  }

  public getPedidoVoucher(id: number): Observable<any> {
    return this.http.request("get", `api/pedido/voucher/${id}`);
  }

  public getPedidoVoucherPorProduto(id: number): Observable<any> {
    return this.http.request("get", `api/pedido/voucher/produtoPreco/${id}`);
  }

  public setCollected(id, code): Observable<any> {
    return this.http.request("post", `api/pedido/${id}/retirar`, code);
  }

  public setAcceptOrder(id): Observable<any> {
    return this.http.request("post", `api/pedido/confirmarPedidoEstabelecimento/${id}`);
  }

  public setRejectOrder(id): Observable<any> {
    return this.http.request("post", `api/pedido/recusarPedidoEstabelecimento/${id}`);
  }

  public setAvailableOrder(id): Observable<any> {
    return this.http.request("post", `api/pedido/disponbilizaPedidoConsumidor/${id}`);
  }

  public setSentOrder(id): Observable<any> {
    return this.http.request("post", `api/pedido/enviarPedidoConsumidor/${id}`);
  }

  private montaDadosPedido(pedido: any) {

    let objRetorno = {
      Id: pedido.Id,
      Cartao: {
        Id: pedido.Cartao.Id,
        Senha: pedido.Cartao.Senha
      },
      Endereco: {
        Id: pedido.Endereco.Id
      },
      Agendamento: null,
      Usuario: {
        Id: HttpProvider.userAuth.UsuarioId
      },
      AvaliacaoPedido: pedido.AvaliacaoPedido,
      Valor: pedido.Valor.replace(".", "").replace(",", "."),
      Senha: pedido.Senha,
      Loja: {
        Id: pedido.Loja.Id
      }
    };

    if (pedido.Agendamento)
      objRetorno.Agendamento = pedido.Agendamento

    return objRetorno;
  }

  private montaDadosPedidoPlanoVenda(pedido: any) {

    let objRetorno = {
      Cartao: {
        Id: pedido.Cartao.Id,
        Senha: pedido.Cartao.Senha
      },
      Endereco: null,
      Agendamento: null,
      ListaCompra: null,
      Usuario: {
        Id: HttpProvider.userAuth.UsuarioId
      },
      Valor: pedido.Valor,
      Loja: {
        Id: pedido.Loja.Id
      }
    };

    if (pedido.Agendamento)
      objRetorno.Agendamento = pedido.Agendamento

    if (pedido.ListaCompra)
      objRetorno.ListaCompra = pedido.ListaCompra

    return objRetorno;
  }

  private montaDadosPedidoVoucher(pedido: any) {

    let objRetorno = {
      Usuario: {
        Id: HttpProvider.userAuth.UsuarioId
      },
      Loja: null,
      Fornecedor: null,
      ProdutoPreco: {
        Id: pedido.ProdutoPreco.Id
      }
    };

    if (pedido.Loja)
    {
      objRetorno.Loja = {
        Id: pedido.Loja.Id
      }
    }

    if (pedido.Fornecedor)
    {
      objRetorno.Fornecedor = {
        Id: pedido.Fornecedor.Id
      }
    }

    return objRetorno;
  }
}
