using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.ViewModels;

namespace Aplicacao
{
    public interface IDepartamentoProdutoAplicacao : IBaseAplicacao<DepartamentoProduto>
    {
        IEnumerable<DepartamentoProdutoViewModel> Listar();
    }

    public class DepartamentoProdutoAplicacao : BaseAplicacao<DepartamentoProduto, IDepartamentoProdutoServico>, IDepartamentoProdutoAplicacao
    {
        private readonly IDepartamentoProdutoServico _servico;
        public DepartamentoProdutoAplicacao(IDepartamentoProdutoServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(DepartamentoProduto entity)
        {
            var grupoRetorno = BuscarPorId(entity.Id) ?? entity;

            grupoRetorno.Id = entity.Id;
            grupoRetorno.Nome = entity.Nome;

            if (string.IsNullOrEmpty(grupoRetorno.Nome)) throw new Exception("Nome precisa ser preenchido!");

            Servico.Salvar(grupoRetorno);
        }

        public IEnumerable<DepartamentoProdutoViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new DepartamentoProdutoViewModel(x)).ToList();
        }
    }
}