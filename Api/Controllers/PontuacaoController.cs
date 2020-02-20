using Api.Models.Pontuacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    [Authorize]
    //[RoutePrefix("api/pontuacao")]
    public class PontuacaoController : ApiController
    {
        private readonly Aplicacao.ApiInfox.ApiTransacaoCartao apiTransacaoCartaoInfox;
        private readonly Dominio.IPessoaServico pessoaServico;
        private readonly Dominio.ICartaoServico cartaoServico;

        public PontuacaoController(
            Aplicacao.ApiInfox.ApiTransacaoCartao apiTransacaoCartaoInfox,
            Dominio.IPessoaServico pessoaServico,
            Dominio.ICartaoServico cartaoServico
        )
        {
            this.apiTransacaoCartaoInfox = apiTransacaoCartaoInfox;
            this.pessoaServico = pessoaServico;
            this.cartaoServico = cartaoServico;
        }

        [HttpPost, Route("api/redemption/perform")]
        public Models.Pontuacao.Response.RedemptionPerform EfetuarResgate(Models.Pontuacao.Request.RedemptionPerform request)
        {
            throw new NotImplementedException();
        }

        [HttpPut, Route("api/redemption/perform/confirm")]
        public Models.Pontuacao.Response.RedemptionConfirm ConfirmarResgate(Models.Pontuacao.Request.RedemptionConfirm request)
        {
            throw new NotImplementedException();
        }

        [HttpPut, Route("api/redemption/perform/cancel")]
        public Models.Pontuacao.Response.RedemptionCancel CancelarResgate(Models.Pontuacao.Request.RedemptionCancel request)
        {
            throw new NotImplementedException();
        }

        [HttpPut, Route("api/redemption/perform/reversal")]
        public Models.Pontuacao.Response.RedemptionReversal EstornoResgate(Models.Pontuacao.Request.RedemptionReversal request)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("api/redemption/perform/reversalComplement")]
        public Models.Pontuacao.Response.BasicResponse ComplementoResgate(Models.Pontuacao.Request.RedemptionComplement request)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("api/credit")]
        public Models.Pontuacao.Response.BasicResponse CreditoPontos(Models.Pontuacao.Request.CreditPerform request)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("api/creditReversal")]
        public Models.Pontuacao.Response.BasicResponse EstornoCredito(Models.Pontuacao.Request.CreditReversal request)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("api/participant/{login}/balance")]
        public decimal ConsultaSaldo(string login)
        {
            //var pessoa = pessoaServico.BuscarPorId(id);
            //var cartaoPesquisa = cartaoServico.DescriptografarCartao


            //    .GetByPessoa(id)?
            //    .Select(x => new Models.CartaoModelView(x))?.ToList()?
            //    .FirstOrDefault(x => x.NumeroSemMascara == login.ExtractLettersAndNumbers());

            //try
            //{
            //    var cartaoRetorno = apiTransacaoCartaoInfox.Saldo(pessoa.CodClienteInfox, cartao.ExtractLettersAndNumbers(), cartaoSenha);
            //    if (cartaoRetorno.Contains(cartao.Substring(0, 4)))
            //    {
            //        return Newtonsoft.Json.JsonConvert.DeserializeObject<Aplicacao.ApiInfox.Models.CartaoModelView>(cartaoRetorno);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //    message.Content = new StringContent($"Ocorreu um erro ao realizar a busca do saldo! Erro:[{ex.Message}]");
            //    throw new HttpResponseException(message);
            //}

            //return null;

            throw new NotImplementedException();
        }

        [HttpGet, Route("api/participant/{login}/balance/onHold")]
        public decimal SaldoEmEspera(string login)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("api/participant/{login}/extract/from/{initialDate}/to/{finalDate}")]
        public Models.Pontuacao.Response.CreditExtract[] ConsultaExtrato(string login, DateTime initialDate, DateTime finalDate)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("api/participant/{login}/extract/consolidated")]
        public Models.Pontuacao.Response.ExtractConsolidated ExtratoConsolidado(string login)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Route("api/participant/{login}/expiration/{expirationDate}")]
        public Models.Pontuacao.Response.CreditExpiration[] Expiracao(string login, DateTime expirationDate)
        {
            throw new NotImplementedException();
        }
    }
}
