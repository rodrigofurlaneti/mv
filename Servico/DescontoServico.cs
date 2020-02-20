using System.Collections.Generic;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IDescontoServico : IBaseServico<Desconto>
    {
        Resposta<Desconto> ValidarDesconto(int id);
        decimal CalcularValorDesconto(decimal totalACalcular, Desconto desconto);
        decimal CalcularValorDesconto(decimal totalACalcular, int id);
    }

    public class DescontoServico : BaseServico<Desconto, IDescontoRepositorio>, IDescontoServico
    {
        private readonly IDescontoRepositorio _descontoRepositorio;
        public DescontoServico(IDescontoRepositorio descontoRepositorio)
        {
            _descontoRepositorio = descontoRepositorio;
        }

        public decimal CalcularValorDesconto(decimal totalACalcular, Desconto desconto)
        {
            if (!ValidacaoDesconto(desconto).Key)
                return 0;

            return desconto == null || desconto.ValorDesconto == 0 
                ? 0
                : desconto.TipoDesconto == TipoDesconto.Percentual
                    ? (totalACalcular * desconto.ValorDesconto) / 100
                    : desconto.ValorDesconto;
        }

        public decimal CalcularValorDesconto(decimal totalACalcular, int id)
        {
            var descontoRetorno = BuscarPorId(id);
            return CalcularValorDesconto(totalACalcular, descontoRetorno);
        }

        public Resposta<Desconto> ValidarDesconto(int id)
        {
            var resposta = new Resposta<Desconto>
            {
                TipoMensagem = TipoModal.Success
            };

            var retorno = BuscarPorId(id);
            resposta.ObjetoRetorno = retorno;

            var validacao = ValidacaoDesconto(retorno);
            if (validacao.Key) return resposta;

            resposta.TipoMensagem = TipoModal.Warning;
            resposta.Mensagem = validacao.Value;

            return resposta;
        }

        private KeyValuePair<bool, string> ValidacaoDesconto(Desconto desconto)
        {
            if (desconto == null || desconto.Id == 0)
                return new KeyValuePair<bool, string>(false, "Desconto não encontrado!");
            return new KeyValuePair<bool, string>(true, string.Empty);
        }
    }
}