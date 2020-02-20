using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface ICupomAplicacao : IBaseAplicacao<Cupom>
    {
        Cupom BuscarCupom(string cupom);
        Resposta<Cupom> ValidarCupom(string cupom);
        decimal CalcularValorCupom(decimal totalACalcular, Cupom cupom);
        decimal CalcularValorCupom(decimal totalACalcular, string cupom);

        IEnumerable<CupomViewModel> Listar();
    }

    public class CupomAplicacao : BaseAplicacao<Cupom, ICupomServico>, ICupomAplicacao
    {
        private readonly ICupomServico _cupomServico;
        public CupomAplicacao(ICupomServico cupomServico)
        {
            _cupomServico = cupomServico;
        }

        public Cupom BuscarCupom(string cupom)
        {
            return _cupomServico.BuscarCupom(cupom);
        }

        public decimal CalcularValorCupom(decimal totalACalcular, Cupom cupom)
        {
            return _cupomServico.CalcularValorCupom(totalACalcular, cupom);
        }

        public decimal CalcularValorCupom(decimal totalACalcular, string cupom)
        {
            return _cupomServico.CalcularValorCupom(totalACalcular, cupom);
        }

        public new void Salvar(Cupom entity)
        {
            var cupomRetorno = BuscarPorId(entity.Id) ?? entity;
            cupomRetorno.CodigoCupom = entity.CodigoCupom;
            cupomRetorno.Descricao = entity.Descricao;
            cupomRetorno.TipoDesconto = entity.TipoDesconto;
            cupomRetorno.ValidadeFinal = entity.ValidadeFinal;
            cupomRetorno.ValorCupom = entity.ValorCupom;
            cupomRetorno.CodigoCupom = entity.CodigoCupom;

            if (string.IsNullOrEmpty(cupomRetorno.CodigoCupom)) throw new Exception("Campo Código não preenchido!");
            if (string.IsNullOrEmpty(cupomRetorno.Descricao)) throw new Exception("Campo Descrição não preenchido!");
            if (cupomRetorno.ValidadeFinal == DateTime.MinValue || cupomRetorno.ValidadeFinal == DateTime.MaxValue) throw new Exception("Campo Validade não preenchido!");
            if (cupomRetorno.ValorCupom == 0) throw new Exception("Campo Valor do cupom dever não preenchido!");
            if ((int)cupomRetorno.TipoDesconto == 0) throw new Exception("Campo Tipo não preenchido!");


            Servico.Salvar(cupomRetorno);
        }

        public Resposta<Cupom> ValidarCupom(string cupom)
        {
            return _cupomServico.ValidarCupom(cupom);
        }

        public IEnumerable<CupomViewModel> Listar()
        {
            return _cupomServico.Buscar().Select(x => new CupomViewModel(x)).ToList();
        }
    }
}