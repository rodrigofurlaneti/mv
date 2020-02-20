using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using ApiInfox.Helpers;
using ApiInfox.Models;
using Aplicacao.apimobiseg;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/pedido")]
    public class PedidoController : ApiController
    {
        private readonly IPedidoServico _pedidoServico;
        private readonly IListaCompraServico _listaCompraServico;
        private readonly ICartaoServico _cartaoServico;
        private readonly IUsuarioServico _usuarioServico;
        private readonly ApiTransacaoCartao _apiTransacaoCartao;

        public PedidoController(IPedidoServico pedidoServico, IListaCompraServico listaCompraServico, ICartaoServico cartaoServico, IUsuarioServico usuarioServico)
        {
            _pedidoServico = pedidoServico;
            _listaCompraServico = listaCompraServico;
            _apiTransacaoCartao = new ApiTransacaoCartao();
            _cartaoServico = cartaoServico;
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
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedidosUsuario = _pedidoServico.BuscarPor(x => x.ListaCompra.Loja.Id == idLoja).Skip(inicio).Take(quantidade);

            var pedidosRetorno = new List<PedidoModelView>();

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

            //entity.Cartao = entity.Cartao != null ? _cartaoServico.DescriptografarCartao(entity.Cartao) : entity.Cartao;

            //entity.ListaCompra = _listaCompraServico.AtribuiDescontoAListaCompra(entity.ListaCompra);

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
        public Resposta<Pedido> Finalizar([FromBody] Pedido entity)
        {
            entity.ListaCompra = _listaCompraServico.BuscarPorId(entity.ListaCompra.Id);

            var pedido = _pedidoServico.ValidarESalvar(entity);
            pedido.Cartao = pedido.Cartao == null ? pedido.Cartao : _cartaoServico.DescriptografarCartao(pedido.Cartao);

            var cpf = pedido.Usuario.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Cpf)?.Numero;
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
                        //MobSeg
                        retornoApi = _apiTransacaoCartao.RealizaTransacaoCartao(Convert.ToInt64(cpf?.Replace(".", "").Replace("-", "")), 0,
                            pedido.Cartao?.NumeroSemMascara, pedido.Cartao?.Validade.Replace("/", ""), pedido.Cartao?.Cvv, pedido.ListaCompra.Total.ToString("######.00").Replace(",", "").Replace(".", ""));

                        retorno = new JavaScriptSerializer().Deserialize<RespostaTransacaoCartaoModelView>(retornoApi);
                        break;
                    case "1":
                        //PayZen
                        var retornoKeyValue = Aplicacao.apipayzen.ApiPayzenBase.CreatePayment(pedido);
                        retornoApi = retornoKeyValue.Value;
                        retorno = new RespostaTransacaoCartaoModelView
                        {
                            CodigoResposta = retornoKeyValue.Key,
                            Mensagem = retornoKeyValue.Value
                        };
                        break;
                    default:
                        retornoApi = "sucesso";
                        break;
                }

                pedido.Cartao = _cartaoServico.Criptografar(_cartaoServico.BuscarPorId(pedido.Cartao.Id));
            }

            if (retornoApiInfox.Contains("sucesso"))
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

            return new Resposta<Pedido>
            {
                Mensagem = retornoApiInfox.Contains("sucesso") ? "Compra realizada com sucesso!" : "Falha ao processar o pagamento com a operadora do cartão!",
                ObjetoRetorno = pedidoRetorno,
                TipoMensagem = retornoApiInfox.Contains("sucesso") ? TipoModal.Success : TipoModal.Danger
            };
        }

        [HttpPost]
        [Route("confirmarPedidoEstabelecimento/{pedidoId}")]
        public void ConfirmarPedidoEstabelecimento(int pedidoId)
        {
            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
                throw new Exception("Não foi possível obter o usuário logado.");

            var pedido = _pedidoServico.BuscarPorId(pedidoId);

            _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.EmSeparacao, usuario);
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
    }
}