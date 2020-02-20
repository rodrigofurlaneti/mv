using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;
using Api.Helpers;
using Api.Models;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Core.Extensions;
using Core.Exceptions;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/pedido")]
    public class PedidoController : ApiController
    {
        private readonly IPedidoServico _pedidoServico;
        private readonly IListaCompraServico _listaCompraServico;
        private readonly ICartaoServico _cartaoServico;
        private readonly ITerminalCobrancaLojaServico terminalCobrancaLojaServico;
        private readonly IUsuarioServico _usuarioServico;
        private readonly Aplicacao.apimobiseg.ApiTransacaoCartao _apiTransacaoCartao;
        private readonly Aplicacao.ApiInfox.ApiTransacaoCartao _apiTransacaoCartaoInfox;
        private readonly Aplicacao.apiebanx.ApiTransacaoCartao _apiTransacaoEbanx;

        public PedidoController(
            IPedidoServico pedidoServico, 
            IListaCompraServico listaCompraServico, 
            ICartaoServico cartaoServico, 
            ITerminalCobrancaLojaServico terminalCobrancaLojaServico,
            IUsuarioServico usuarioServico)
        {
            _pedidoServico = pedidoServico;
            _listaCompraServico = listaCompraServico;
            _apiTransacaoCartao = new Aplicacao.apimobiseg.ApiTransacaoCartao();
            _apiTransacaoCartaoInfox = new Aplicacao.ApiInfox.ApiTransacaoCartao();
            _apiTransacaoEbanx = new Aplicacao.apiebanx.ApiTransacaoCartao();
            _cartaoServico = cartaoServico;
            this.terminalCobrancaLojaServico = terminalCobrancaLojaServico;
            _usuarioServico = usuarioServico;
        }

        [HttpGet]
        [Route("")]
        public virtual IEnumerable<PedidoModelView> Get()
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedidosUsuario = _pedidoServico.BuscarPorUsuario(usuario);

            var pedidosRetorno = new List<PedidoModelView>();

            pedidosUsuario.Select(x => x.Cartao = _cartaoServico.DescriptografarCartao(x.Cartao));

            foreach (var pedido in pedidosUsuario)
            {
                pedidosRetorno.Add(new PedidoModelView(pedido));
            }

            return pedidosRetorno;
        }

        [HttpGet]
        [Route("pedidoLoja/{idLoja}/{inicio}/{quantidade}")]
        public virtual IEnumerable<PedidoModelView> BuscarPorLoja(int idLoja, int inicio, int quantidade)
        {
            var pedidosUsuario = _pedidoServico.BuscarPor(x => x.Loja.Id == idLoja).Skip(inicio).Take(quantidade);

            var pedidosRetorno = new List<PedidoModelView>();

            pedidosUsuario.Select(x => x.Cartao = _cartaoServico.DescriptografarCartao(x.Cartao));

            foreach (var pedido in pedidosUsuario)
            {
                pedidosRetorno.Add(new PedidoModelView(pedido));
            }

            return pedidosRetorno.OrderByDescending(x => x.DataInsercao);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual PedidoModelView Get(int id)
        {
            var entity = new PedidoModelView(_pedidoServico.BuscarPorId(id));
            if (entity == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return entity;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/solicitarRetirada")]
        public string SolicitarRetirada(int id)
        {
            return _pedidoServico.SolicitarRetirada(id);
        }

        [HttpPost]
        [Route("finalizar")]
        public Resposta<PedidoModelView> Finalizar([FromBody] Pedido entity)
        {
            if (entity.ListaCompra != null)
                entity.ListaCompra = _listaCompraServico.BuscarPorId(entity.ListaCompra.Id);

            entity.Usuario = _usuarioServico.BuscarPorId(entity.Usuario.Id);

            Pessoa pedidoPessoa = entity.Usuario.Pessoa;
            
            var senhaCartao = entity.Cartao.Senha;
            var pedido = _pedidoServico.ValidarESalvar(entity);

            pedido.Cartao = pedido.Cartao == null ? pedido.Cartao : _cartaoServico.DescriptografarCartao(pedido.Cartao);

            if (terminalCobrancaLojaServico.PrimeiroPor(t => t.Usuario.Id == entity.Usuario.Id) != null) //Usuário do pedido é um terminal
                entity.Usuario.Pessoa = pedidoPessoa;

            string retornoApi;
            var retorno = new RespostaTransacaoCartaoModelView();

            if (ConfigurationManager.AppSettings["API_REALIZA_TRANSACAO"] == "0")
                retornoApi = "sucesso";
            else
            {
                pedido.Cartao.Decrypted = false;
                pedido.Cartao = _cartaoServico.DescriptografarCartao(pedido.Cartao);

                switch (ConfigurationManager.AppSettings["API_REALIZA_TRANSACAO_FORMA_PAGAMENTO"])
                {
                    case "0":
                        {
                            //MobSeg
                            var cpf = entity.Usuario.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Cpf)?.Numero;

                            retornoApi = _apiTransacaoCartao.RealizaTransacaoCartao(Convert.ToInt64(cpf?.Replace(".", "").Replace("-", "")), 0,
                                pedido.Cartao?.NumeroSemMascara, pedido.Cartao?.Validade.Replace("/", ""), pedido.Cartao?.Cvv, pedido.ListaCompra.Total.ToString("######.00").Replace(",", "").Replace(".", ""));

                            retorno = new JavaScriptSerializer().Deserialize<RespostaTransacaoCartaoModelView>(retornoApi);
                        }
                        break;
                    case "1":
                        {
                            //PayZen
                            var retornoKeyValue = Aplicacao.apipayzen.ApiPayzenBase.CreatePayment(pedido);
                            retornoApi = retornoKeyValue.Value;
                            retorno = new RespostaTransacaoCartaoModelView
                            {
                                CodigoResposta = retornoKeyValue.Key,
                                Mensagem = retornoKeyValue.Value
                            };
                        }
                        break;
                    case "2":
                        {
                            //MeuVale
                            var cpf = entity.Usuario.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Cpf)?.Numero;

                            pedido.CodEstabelecimentoInfox = pedido.Loja.CodigoInfox;
                            var pedidoModel = new Aplicacao.ApiInfox.Models.PedidoCompraModelView
                            {
                                Cliente = new Aplicacao.ApiInfox.Models.ClienteModelView
                                {
                                    Id = entity.Usuario.Pessoa.CodClienteInfox,
                                    CodCliente = entity.Usuario.Pessoa.CodClienteInfox,
                                    Pessoa = new Aplicacao.ApiInfox.Models.PessoaModelView
                                    {
                                        Cpf = cpf?.Replace(".", "").Replace("-", ""),
                                        DataNascimento = entity.Usuario.Pessoa.DataNascimento
                                    },
                                    Cartao = new Aplicacao.ApiInfox.Models.CartaoModelView
                                    {
                                        NomeImpresso = pedido.Cartao?.NomeImpresso,
                                        Numero = pedido.Cartao?.NumeroSemMascara.ExtractLettersAndNumbers(),
                                        Cvv = pedido.Cartao?.Cvv,
                                        Senha = senhaCartao
                                    }
                                },
                                ValorPedido = pedido.Valor.ToString("F2").Replace(",", "").Replace(".", ""),
                                CodEstabelecimento = pedido.CodEstabelecimentoInfox
                            };
                            retornoApi = _apiTransacaoCartaoInfox.CompraAVista(new JavaScriptSerializer().Serialize(pedidoModel));

                            var responseJson = new JavaScriptSerializer().Deserialize<Aplicacao.ApiInfox.Models.ResponseModelView>(retornoApi);

                            pedido.NSURedeRXInfox = responseJson.NSU_RedeRX;

                            pedido.Cartao = _cartaoServico.Criptografar(_cartaoServico.BuscarPorId(pedido.Cartao.Id));
                            _pedidoServico.Salvar(pedido);

                            retorno = new RespostaTransacaoCartaoModelView
                            {
                                CodigoResposta = responseJson.CodResp,
                                Mensagem = responseJson.StrResp
                            };

                            if (responseJson.Return > 0)
                            {
                                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoNaoAprovado, pedido.Usuario, responseJson.StrResp, responseJson.Return.ToString());

                                //throw new BusinessRuleException($"Resultado: {responseJson.Return} - {responseJson.StrResp}");
                                return new Resposta<PedidoModelView>
                                {
                                    Mensagem = ($"Falha ao processar o pagamento: { responseJson.Return } - { responseJson.StrResp }"),
                                    ObjetoRetorno = new PedidoModelView(pedido),
                                    TipoMensagem = TipoModal.Danger
                                };
                            }

                            retorno = new RespostaTransacaoCartaoModelView
                            {
                                CodigoResposta = responseJson.CodResp,
                                Mensagem = responseJson.StrResp
                            };

                            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoAprovado, pedido.Usuario, retorno.Mensagem, retorno.CodigoResposta);

                            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoAvaliacao, pedido.Usuario, "Sua avaliação é muito importante para nós");

                            return new Resposta<PedidoModelView>
                            {
                                Mensagem = responseJson.Return == 0 ? "Pagamento realizado com sucesso! Loja: " + pedido.Loja.Descricao + " Valor: R$ " + pedido.Valor.ToString("N2") : "Falha ao processar o pagamento com a operadora do cartão!",
                                ObjetoRetorno = new PedidoModelView(pedido),
                                TipoMensagem = responseJson.Return == 0 ? TipoModal.Success : TipoModal.Danger
                            };
                        }
                    case "3":
                        {
                            //Ebanx
                            var retornoEbanx = _apiTransacaoEbanx.CompraDireta(pedido);
                            var retornoJson = Newtonsoft.Json.JsonConvert.SerializeObject(retornoEbanx);
                            
                            retornoApi = retornoJson;

                            pedido.Cartao = _cartaoServico.Criptografar(_cartaoServico.BuscarPorId(pedido.Cartao.Id));
                            _pedidoServico.Salvar(pedido);

                            retorno = new RespostaTransacaoCartaoModelView
                            {
                                CodigoResposta = retornoEbanx.Code,
                                Mensagem = retornoEbanx.Message
                            };

                            if (retorno.CodigoResposta != "OK")
                            {
                                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoNaoAprovado, pedido.Usuario, retorno.Mensagem, retorno.CodigoResposta);
                                throw new BusinessRuleException($"Resultado: {retorno.CodigoResposta} - {retorno.Mensagem}");
                            }

                            _pedidoServico.ModificaStatus(pedido.Id, StatusPedido.AguardandoPagamento, pedido.Usuario, codigoRetornoTransacao: retornoEbanx.payment?.hash);
                            
                            return new Resposta<PedidoModelView>
                            {
                                Mensagem = retorno.CodigoResposta == "OK" ? "Pagamento solicitado com sucesso!" : "Falha ao processar o pagamento com a operadora do cartão!",
                                ObjetoRetorno = new PedidoModelView(pedido),
                                TipoMensagem = retorno.CodigoResposta == "OK" ? TipoModal.Success : TipoModal.Danger
                            };
                        }
                    default:
                        retornoApi = "sucesso";
                        break;
                }

                pedido.Cartao = _cartaoServico.Criptografar(_cartaoServico.BuscarPorId(pedido.Cartao.Id));
            }

            if (retornoApi.Contains("sucesso"))
            {
                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoAprovado, pedido.Usuario, retorno.Mensagem, retorno.CodigoResposta);

                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoConfirmacao, pedido.Usuario, retorno.Mensagem, retorno.CodigoResposta);

            }
            else
                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoNaoAprovado, pedido.Usuario, retorno.Mensagem, retorno.CodigoResposta);

            var pedidoRetorno = _pedidoServico.BuscarPorId(pedido.Id);
            pedidoRetorno.ListaCompra = _listaCompraServico.AtribuiDescontoAListaCompra(pedidoRetorno.ListaCompra);

            pedidoRetorno.Cartao.Decrypted = false;
            pedidoRetorno.Cartao = _cartaoServico.DescriptografarCartao(pedidoRetorno.Cartao);
            pedidoRetorno.AvaliacaoPedido = new AvaliacaoPedido { ItensDeAcordoComAnuncio = false, NotaAplicativo = 0, NotaPedido = 0 };

            return new Resposta<PedidoModelView>
            {
                Mensagem = retornoApi.Contains("sucesso") ? "Compra realizada com sucesso!" : "Falha ao processar o pagamento com a operadora do cartão!",
                ObjetoRetorno = new PedidoModelView(pedidoRetorno),
                TipoMensagem = retornoApi.Contains("sucesso") ? TipoModal.Success : TipoModal.Danger
            };
        }

        [HttpGet, Route("{id}/recibo")]
        public string ObterReciboTransacao(int id)
        {
            var pedido = _pedidoServico.BuscarPorId(id);
            var statusAprovado = pedido.ListaHistorico.OrderByDescending(hp => hp.DataInsercao).FirstOrDefault(h => h.StatusPedido == StatusPedido.PagamentoAprovado);
            if (statusAprovado != null)
                return statusAprovado.Descricao.Replace("@", "\n");

            throw new NotFoundException("Recibo não encontrado!");
        }

        [HttpPost]
        [Route("confirmarPedidoEstabelecimento/{pedidoId}")]
        public void ConfirmarPedidoEstabelecimento(int pedidoId)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoAvaliacao, usuario);
        }

        [HttpPost]
        [Route("recusarPedidoEstabelecimento/{pedidoId}")]
        public void RecusarPedidoEstabelecimento(int pedidoId)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PedidoRecusado, usuario);
        }

        [HttpPost]
        [Route("disponbilizaPedidoConsumidor/{pedidoId}")]
        public void DisponbilizaPedidoConsumidor(int pedidoId)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            if (pedido.Agendamento != null)
                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoRetirada, usuario);
            else
                _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.EmSeparacao, usuario);
        }

        [HttpPost]
        [Route("enviarPedidoConsumidor/{pedidoId}")]
        public void EnviarPedidoConsumidor(int pedidoId)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PedidoEnviado, usuario);
        }

        [HttpPost]
        [Route("finalizarpedidoPcSist")]
        private void FinalizarPedidoPcSist(int pedidoId)
        {
            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoRetirada, pedido.Usuario);

        }

        // TODO: Adicionar basic authentication para comunicar com o pdv
        [AllowAnonymous]
        [HttpPost]
        [Route("{id}/retirar/")]
        public void Retirar(int id, [FromBody] string code)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            _pedidoServico.Retirar(id, code, usuario);
        }

        [HttpPost]
        [Route("{id}/status/{status}/usuario/{idUsuario}")]
        public void AtribuiStatus(int id, int status, int idUsuario)
        {
            var usuario = _usuarioServico.BuscarPorId(idUsuario);
            _pedidoServico.AtribuiStatus(id, status, usuario);
        }

        [HttpPost]
        [Route("salvarAvaliacaoPedido/{pedidoId}")]
        public void salvarPedidoComAvaliacao(int pedidoId, [FromBody] AvaliacaoPedido avaliacaoPedido)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            pedido.AvaliacaoPedido = avaliacaoPedido;

            _pedidoServico.SalvarPedidoComAvaliacao(pedido);
        }

        //Criar chama de Estorno
        [HttpPost]
        [Route("estornar/{pedidoId}")]
        public Resposta<Pedido> EstornarPedido(int pedidoId)
        {
            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            pedido.Cartao.Decrypted = false;
            pedido.Cartao = _cartaoServico.DescriptografarCartao(pedido.Cartao);

            var pedidoModel = new Aplicacao.ApiInfox.Models.PedidoCompraModelView
            {
                Cliente = new Aplicacao.ApiInfox.Models.ClienteModelView
                {
                    Id = pedido.Usuario.Pessoa.CodClienteInfox,
                    CodCliente = pedido.Usuario.Pessoa.CodClienteInfox,
                    Cartao = new Aplicacao.ApiInfox.Models.CartaoModelView
                    {
                        Numero = pedido?.Cartao?.NumeroSemMascara.ExtractLettersAndNumbers(),
                    }
                },
                NSU_RedeRX = pedido.NSURedeRXInfox,
                CodEstabelecimento = pedido.CodEstabelecimentoInfox
            };
            var retornoApi = _apiTransacaoCartaoInfox.DesfazCompraAVista(new JavaScriptSerializer().Serialize(pedidoModel));

            var responseJson = new JavaScriptSerializer().Deserialize<Aplicacao.ApiInfox.Models.ResponseModelView>(retornoApi);

            var retorno = new RespostaTransacaoCartaoModelView
            {
                CodigoResposta = responseJson.CodResp,
                Mensagem = responseJson.Return == 0 ? $"Estorno realizado com sucesso. {responseJson.StrResp}" : responseJson.StrResp
            };

            return new Resposta<Pedido>
            {
                Mensagem = responseJson.Return == 0 ? "Estorno realizado com sucesso!" : "Falha ao estornar!",
                ObjetoRetorno = pedido,
                TipoMensagem = responseJson.Return == 0 ? TipoModal.Success : TipoModal.Danger
            };
        }

        [HttpPost]
        [Route("finalizarVoucher")]
        public Resposta<PedidoVoucherModelView> FinalizarPedidoVoucher([FromBody] PedidoVoucher entity)
        {
            var pedidoVoucherServico = ServiceLocator.Current.GetInstance<IPedidoVoucherServico>();

            entity.Usuario = _usuarioServico.BuscarPorId(entity.Usuario.Id);
            var pedido = pedidoVoucherServico.ValidarESalvar(entity);

            pedidoVoucherServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoRetirada, pedido.Usuario, "Voucher gerado com sucesso!");
            
            return new Resposta<PedidoVoucherModelView>
            {
                Mensagem = "Voucher gerado com sucesso!",
                ObjetoRetorno = new PedidoVoucherModelView(pedido),
                TipoMensagem = TipoModal.Success
            };
        }

        [HttpGet]
        [Route("voucher/{id}")]
        public virtual PedidoVoucherModelView GetPedidoVoucher(int id)
        {
            var pedidoVoucherServico = ServiceLocator.Current.GetInstance<IPedidoVoucherServico>();

            var entity = new PedidoVoucherModelView(pedidoVoucherServico.BuscarPorId(id));
            if (entity == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            return entity;
        }

        [HttpGet]
        [Route("voucher/produtoPreco/{id}")]
        public virtual PedidoVoucherModelView GetPedidoVoucherPorProdutoPreco(int id)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedidoVoucherServico = ServiceLocator.Current.GetInstance<IPedidoVoucherServico>();

            var pedido = pedidoVoucherServico.BuscarPor(x => x.ProdutoPreco.Id == id && x.Usuario.Id == usuario.Id).FirstOrDefault();

            if (pedido == null || pedido.Status != StatusPedido.AguardandoRetirada) return null; 

            var entity = new PedidoVoucherModelView(pedido);
            if (entity == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            return entity;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{id}/resgatarVoucher/")]
        public void ResgatarVoucher(int id, [FromBody] string code)
        {
            var pedidoVoucherServico = ServiceLocator.Current.GetInstance<IPedidoVoucherServico>();

            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            pedidoVoucherServico.Retirar(id, code, usuario);
        }
    }
}