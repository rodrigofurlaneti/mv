using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface ICupomServico : IBaseServico<Cupom>
    {
        Cupom BuscarCupom(string cupom);
        Resposta<Cupom> ValidarCupom(string cupom);
        decimal CalcularValorCupom(decimal totalACalcular, Cupom cupom);
        decimal CalcularValorCupom(decimal totalACalcular, string cupom);
    }

    public class CupomServico : BaseServico<Cupom, ICupomRepositorio>, ICupomServico
    {
        private readonly ICupomRepositorio _cupomRepositorio;
        public CupomServico(ICupomRepositorio cupomRepositorio)
        {
            _cupomRepositorio = cupomRepositorio;
        }

        public Cupom BuscarCupom(string cupom)
        {
            return _cupomRepositorio.BuscarCupom(cupom);
        }

        public decimal CalcularValorCupom(decimal totalACalcular, Cupom cupom)
        {
            if (!ValidacaoCupom(cupom).Key)
                return 0;

            return cupom == null || cupom.ValorCupom == 0
                ? 0
                : cupom.TipoDesconto == TipoDesconto.Percentual
                    ? (totalACalcular * cupom.ValorCupom) / 100
                    : cupom.ValorCupom;
        }

        public decimal CalcularValorCupom(decimal totalACalcular, string cupom)
        {
            var cupomRetorno = BuscarCupom(cupom);
            return CalcularValorCupom(totalACalcular, cupomRetorno);
        }

        public Resposta<Cupom> ValidarCupom(string cupom)
        {
            var resposta = new Resposta<Cupom>
            {
                TipoMensagem = TipoModal.Success
            };

            var retorno = BuscarCupom(cupom);
            resposta.ObjetoRetorno = retorno;

            var validacao = ValidacaoCupom(retorno);
            if (validacao.Key) return resposta;

            resposta.TipoMensagem = TipoModal.Warning;
            resposta.Mensagem = validacao.Value;

            return resposta;
        }

        private KeyValuePair<bool, string> ValidacaoCupom(Cupom cupom)
        {
            if (cupom == null || cupom.Id == 0)
                return new KeyValuePair<bool, string>(false, "Cupom Não Encontrado!");
            if (cupom.ValidadeFinal.Date < DateTime.Now.Date)
                return new KeyValuePair<bool, string>(false, "Cupom Expirado!");

            return new KeyValuePair<bool, string>(true, string.Empty);
        }
    }
}