using Core.Exceptions;
using Dominio;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    [AllowAnonymous, RoutePrefix("api/ebanx")]
    public class EbanxController : ApiController
    {
        private readonly IPedidoServico pedidoServico;
        private readonly ICartaoServico cartaoServico;
        private readonly IUsuarioServico usuarioServico;
        private readonly ILojaServico lojaServico;
        private readonly Aplicacao.apiebanx.ApiTransacaoCartao _apiTransacaoEbanx;

        public EbanxController(
            IPedidoServico pedidoServico,
            ICartaoServico cartaoServico,
            IUsuarioServico usuarioServico,
            ILojaServico lojaServico
        )
        {
            this.pedidoServico = pedidoServico;
            this.cartaoServico = cartaoServico;
            this.usuarioServico = usuarioServico;
            this.lojaServico = lojaServico;
            _apiTransacaoEbanx = new Aplicacao.apiebanx.ApiTransacaoCartao();
        }

        [HttpPost, Route("comprar")]
        public IHttpActionResult Comprar(string cnpjLoja, string cpfUsuario, int cartaoId, decimal valor, bool auto_capture = true)
        {
            cpfUsuario = cpfUsuario.Trim().Replace(".", "").Replace("-", "").PadLeft(11, '0');
            cnpjLoja = cnpjLoja.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            var cartao = cartaoServico.BuscarPorId(cartaoId);
            var usuario = usuarioServico.PrimeiroPor(u => u.Pessoa.Documentos.Any(d => d.Numero.Trim().Replace(".", "").Replace("-", "").Equals(cpfUsuario)));
            var loja = lojaServico.PrimeiroPor(l => l.Cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "").Equals(cnpjLoja));

            if (cartao == null) throw new BusinessRuleException("Cartão não encontrado.");
            if (usuario == null) throw new BusinessRuleException("Usuário não encontrado.");
            if (loja == null) throw new BusinessRuleException("Loja não encontrada.");

            var pedido = new Entidade.Pedido
            {
                Cartao = cartao,
                Usuario = usuario,
                Valor = valor,
                Loja = loja,
                Endereco = usuario?.Pessoa?.EnderecoResidencial
            };
            pedidoServico.ValidarESalvar(pedido);

            var response = _apiTransacaoEbanx.CompraDireta(pedido, auto_capture);

            pedidoServico.ModificaStatus(pedido.Id, StatusPedido.AguardandoPagamento, pedido.Usuario, codigoRetornoTransacao: response.payment?.hash);

            return Ok(response);
        }

        [HttpPost, Route("cancelar")]
        public IHttpActionResult Cancelar(string hash)
        {
            var pedido = pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));
            if (pedido == null) throw new BusinessRuleException("Pedido não encontrado.");

            var response = _apiTransacaoEbanx.Cancelar(pedido);
            return Ok(response);
        }

        [HttpPost, Route("cancelarOuEstornar")]
        public IHttpActionResult CancelarOuEstornar(string hash)
        {
            var pedido = pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));
            if (pedido == null) throw new BusinessRuleException("Pedido não encontrado.");

            var response = _apiTransacaoEbanx.CancelarOuEstornar(pedido, "Cancelado a pedido do cliente");
            return Ok(response);
        }

        [HttpPost, Route("estornar")]
        public IHttpActionResult Estornar(string hash)
        {
            var pedido = pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));
            if (pedido == null) throw new BusinessRuleException("Pedido não encontrado.");

            var response = _apiTransacaoEbanx.Estornar(pedido, "Cancelado a pedido do cliente");
            return Ok(response);
        }

        [HttpGet, Route("imprimir")]
        public HttpResponseMessage Imprimir(string hash)
        {
            var pedido = pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));
            if (pedido == null) throw new BusinessRuleException("Pedido não encontrado.");

            var response = _apiTransacaoEbanx.Imprimir(pedido);
            return Request.CreateResponse(HttpStatusCode.OK, response, "text/html");
        }

        [HttpGet, Route("consultar")]
        public IHttpActionResult Consultar(string hash)
        {
            var pedido = pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));
            if (pedido == null) throw new BusinessRuleException("Pedido não encontrado.");

            var response = _apiTransacaoEbanx.Consulta(hash);
            return Ok(response);
        }

        [HttpPost, Route("capturar")]
        public IHttpActionResult Capturar(string hash, decimal? valor = null)
        {
            var pedido = pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));
            if (pedido == null) throw new BusinessRuleException("Pedido não encontrado.");

            var response = _apiTransacaoEbanx.Capturar(pedido, valor);
            return Ok(response);
        }

        [HttpPost, Route("tokenize")]
        public IHttpActionResult ObterToken(int cartaoId)
        {
            var cartao = cartaoServico.BuscarPorId(cartaoId);
            if (cartao == null) throw new BusinessRuleException("Cartão não encontrado.");

            var response = _apiTransacaoEbanx.TokenCartao(cartao);
            cartao.Token = response.token;

            cartaoServico.ValidaESalva(cartao);

            return Ok(response);
        }

    }
}
