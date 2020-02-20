using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.ViewModels;

namespace Aplicacao
{
    public interface ISubGrupoProdutoAplicacao : IBaseAplicacao<SubGrupoProduto>
    {
        IEnumerable<SubGrupoProdutoViewModel> Listar();
        IEnumerable<SubGrupoProdutoViewModel> Listar(int idGrupo);
    }

    public class SubGrupoProdutoAplicacao : BaseAplicacao<SubGrupoProduto, ISubGrupoProdutoServico>, ISubGrupoProdutoAplicacao
    {
        private readonly ISubGrupoProdutoServico _servico;
        public SubGrupoProdutoAplicacao(ISubGrupoProdutoServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(SubGrupoProduto entity)
        {
            var grupoRetorno = BuscarPorId(entity.Id) ?? entity;

            grupoRetorno.Id = entity.Id;
            grupoRetorno.Nome = entity.Nome;

            if (string.IsNullOrEmpty(grupoRetorno.Nome)) throw new Exception("Campos preenchidos incorretamente!");

            Servico.Salvar(grupoRetorno);
        }

        public IEnumerable<SubGrupoProdutoViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new SubGrupoProdutoViewModel(x)).ToList();
        }

        public IEnumerable<SubGrupoProdutoViewModel> Listar(int idGrupo)
        {
            return _servico.Buscar().Where(x => x.Grupo.Id == idGrupo).Select(x => new SubGrupoProdutoViewModel(x)).ToList();
        }
    }
}