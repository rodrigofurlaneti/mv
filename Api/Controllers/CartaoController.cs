using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Core.Exceptions;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/cartao")]
    public class CartaoController : ApiController
    {
        private readonly ICartaoServico _cartaoServico;
        private readonly IPessoaServico _pessoaServico;
        private readonly IPedidoServico _pedidoServico;
        private readonly Aplicacao.ApiInfox.ApiTransacaoCartao _apiTransacaoCartaoInfox;
        private readonly Aplicacao.apiebanx.ApiTransacaoCartao _apiTransacaoCartaoEbanx;

        public CartaoController(
            ICartaoServico cartaoServico, 
            IPessoaServico pessoaServico,
            IPedidoServico pedidoServico
        )
        {
            _cartaoServico = cartaoServico;
            _pessoaServico = pessoaServico;
            _pedidoServico = pedidoServico;
            _apiTransacaoCartaoInfox = new Aplicacao.ApiInfox.ApiTransacaoCartao();
            _apiTransacaoCartaoEbanx = new Aplicacao.apiebanx.ApiTransacaoCartao();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Cartao entity)
        {
            if ((ConfigurationManager.AppSettings["API_REALIZA_TRANSACAO_FORMA_PAGAMENTO"]?.Equals("3") ?? false) && string.IsNullOrEmpty(entity.Token)) // Ebanx
            {
                entity.NomeImpresso = entity.NomeImpresso ?? Helpers.LoggedUser.GetLoggedUser()?.Pessoa?.Nome?.ToUpper(); // Workaround para falta de nome impresso no cartão

                var tokenResponse = _apiTransacaoCartaoEbanx.TokenCartao(entity);
                if (tokenResponse.status.Equals("SUCCESS"))
                    entity.Token = tokenResponse.token;
            }

            _cartaoServico.ValidaESalva(entity);
            
            return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString()) + entity.Id), entity.Id);
        }

        [HttpPost]
        [Route("{id:int}")]
        public void Update(int id, [FromBody] Cartao entity)
        {
            try
            {
                // Set id in object
                entity.Id = id;
                if (id > 0)
                {
                    var cartao = _cartaoServico.BuscarPorId(id);
                    if (entity.Numero.Contains("X"))
                        entity.Numero = cartao.Numero;
                    if (entity.Cvv.Contains("X"))
                        entity.Cvv = cartao.Cvv;
                }
                //
                _cartaoServico.ValidaESalva(entity);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("deletar/{id:int}")]
        public void Delete(int id)
        {
            try
            {
                _cartaoServico.ExcluirPorId(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("pessoa/{id:int}")]
        public IEnumerable<Models.CartaoModelView> GetByPessoa(int id)
        {
            try
            {
                var pessoa = _pessoaServico.BuscarPorId(id);
                var cartoes = _cartaoServico.GetByPessoa(id)?.Select(x => new Models.CartaoModelView(x))?.ToList();

                //Como não armazenamos a senha do cartão então no caso não pode ser consultado o saldo dos cartões neste momento
                //foreach (var itemCartao in cartoes)
                //{
                //    try
                //    {
                //        var cartaoRetorno = _apiTransacaoCartaoInfox.Saldo(pessoa.Id.ToString(), itemCartao.NumeroSemMascara.ExtractLettersAndNumbers(), itemCartao.Senha);
                //        if (cartaoRetorno.Contains("SALDO"))
                //        {
                //            var card = new JavaScriptSerializer().Deserialize<Aplicacao.ApiInfox.Models.CartaoModelView>(cartaoRetorno);
                //            itemCartao.SaldoDisponivel = card.SaldoDisponivel;
                //            itemCartao.LimiteCredito = card.LimiteCredito;
                //            itemCartao.DiaVencimento = card.DiaVencimento;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        var message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                //        message.Content = new StringContent($"Ocorreu um erro ao realizar a busca do saldo! Erro:[{ex.Message}]");
                //        throw new HttpResponseException(message);
                //    }
                //}

                return cartoes;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("saldo/{id}/{cartao}/{cartaoSenha}")]
        public Aplicacao.ApiInfox.Models.CartaoModelView GetSaldoByCartao(int id, string cartao, string cartaoSenha)
        {
            try
            {
                var pessoa = _pessoaServico.BuscarPorId(id);
                var cartaoPesquisa = _cartaoServico.GetByPessoa(id)?
                    .Select(x => new Models.CartaoModelView(x))?.ToList()?
                    .FirstOrDefault(x => x.NumeroSemMascara == cartao.ExtractLettersAndNumbers());

                try
                {
                    var cartaoRetorno = _apiTransacaoCartaoInfox.Saldo(pessoa.CodClienteInfox, cartao.ExtractLettersAndNumbers(), cartaoSenha);
                    if (cartaoRetorno.Contains(cartao.Substring(0,4)))
                    {
                        return new JavaScriptSerializer().Deserialize<Aplicacao.ApiInfox.Models.CartaoModelView>(cartaoRetorno);
                    }
                }
                catch (Exception ex)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    message.Content = new StringContent($"Ocorreu um erro ao realizar a busca do saldo! Erro:[{ex.Message}]");
                    throw new HttpResponseException(message);
                }

                return null;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("extrato/{id}/{cartao}/{cartaoSenha}/{codEstabelecimento}")]
        public Aplicacao.ApiInfox.Models.ExtratoModelView GetExtratoByCartao(int id, string cartao, string cartaoSenha, string codEstabelecimento)
        {
            try
            {
                var pessoa = _pessoaServico.BuscarPorId(id);
                var cartaoPesquisa = _cartaoServico.GetByPessoa(id)?
                    .Select(x => new Models.CartaoModelView(x))?.ToList()?
                    .FirstOrDefault(x => x.NumeroSemMascara == cartao.ExtractLettersAndNumbers());
                try
                {
                    var extratoRetorno = _apiTransacaoCartaoInfox.Extrato(pessoa.CodClienteInfox, cartao.ExtractLettersAndNumbers(), cartaoSenha, codEstabelecimento);
                    if (extratoRetorno.Contains(cartao.Substring(0,4)))
                    {
                        return new JavaScriptSerializer().Deserialize<Aplicacao.ApiInfox.Models.ExtratoModelView>(extratoRetorno);
                    }
                }
                catch (Exception ex)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    message.Content = new StringContent($"Ocorreu um erro ao realizar o extrato! Erro:[{ex.Message}]");
                    throw new HttpResponseException(message);
                }
                return null;
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("pessoa/{id}/parcialbloq")]
        public IEnumerable<Cartao> GetCartoesParcialBloqPorPessoa(int id)
        {
            try
            {
                return _cartaoServico.GetCartoesParcialBloqPorPessoa(id);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost, Route("ebanx/status_changed"), AllowAnonymous]
        public IHttpActionResult UpdatePagamento(string operation, string notification_type, string hash_codes)
        {
            /*
             The payment status. The following statuses are available:
                OP: the customer has not yet filled the payment details on the EBANX Checkout. It can change either to CA (time out) or PE.
                PE: the payment is pending confirmation. It can change either to CA or CO.
                CO: the payment is confirmed (paid).
                CA: the payment is cancelled.
             */

            var hash_list = hash_codes.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var hash in hash_list)
            {
                var pedido = _pedidoServico.PrimeiroPor(p => p.ListaHistorico.Any(lh => lh.StatusPedido == StatusPedido.AguardandoPagamento && lh.CodigoRetornoTransacao == hash));

                var consultaResponse = _apiTransacaoCartaoEbanx.Consulta(hash: hash);
                if (consultaResponse.payment.status.Equals("PE"))
                {
                    _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoConfirmacao, pedido.Usuario, consultaResponse.Message, consultaResponse.Code);
                }
                else if (consultaResponse.payment.status.Equals("CO"))
                {
                    _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoAprovado, pedido.Usuario, "Pagamento aprovado", consultaResponse.Code + "-" + consultaResponse.Message);
                    _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.AguardandoAvaliacao, pedido.Usuario, "Sua avaliação é muito importante para nós");
                }
                else if (consultaResponse.payment.status.Equals("CA"))
                {
                    _pedidoServico.AtribuiStatus(pedido.Id, (int)StatusPedido.PagamentoNaoAprovado, pedido.Usuario, "Pagamento não aprovado", consultaResponse.Code + "-" + consultaResponse.Message);
                }
            }

            return Ok();
        }
    }
}