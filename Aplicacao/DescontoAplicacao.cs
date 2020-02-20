using System;
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IDescontoAplicacao : IBaseAplicacao<Desconto>
    {
        Resposta<Desconto> ValidarDesconto(int id);
        decimal CalcularValorDesconto(decimal totalACalcular, Desconto desconto);
        decimal CalcularValorDesconto(decimal totalACalcular, int id);
        void Excluir(int id);
    }

    public class DescontoAplicacao : BaseAplicacao<Desconto, IDescontoServico>, IDescontoAplicacao
    {
        private readonly IDescontoServico _descontoServico;
        private readonly IDescontoPessoaServico _descontoPessoaServico;
        public DescontoAplicacao(IDescontoServico descontoServico, IDescontoPessoaServico descontoPessoaServico)
        {
            _descontoServico = descontoServico;
            _descontoPessoaServico = descontoPessoaServico;
        }
        
        public decimal CalcularValorDesconto(decimal totalACalcular, Desconto desconto)
        {
            return _descontoServico.CalcularValorDesconto(totalACalcular, desconto);
        }

        public decimal CalcularValorDesconto(decimal totalACalcular, int id)
        {
            return _descontoServico.CalcularValorDesconto(totalACalcular, id);
        }

        public void Excluir(int id)
        {
            //Remove all dependents
            foreach (var item in _descontoPessoaServico.BuscarPor(x=> x.Desconto.Id == id))
                _descontoPessoaServico.ExcluirPorId(item.Id);

            //Remove item
            _descontoServico.ExcluirPorId(id);
        }

        public new void Salvar(Desconto entity)
        {
            var descontoRetorno = BuscarPorId(entity.Id) ?? entity;
            descontoRetorno.TipoDesconto = entity.TipoDesconto;
            descontoRetorno.ValorDesconto = entity.ValorDesconto;

            Servico.Salvar(descontoRetorno);
        }

        public Resposta<Desconto> ValidarDesconto(int id)
        {
            return _descontoServico.ValidarDesconto(id);
        }
    }
}