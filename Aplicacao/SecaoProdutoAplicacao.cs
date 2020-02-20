using System;
using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;
using Aplicacao.ViewModels;
using System.Linq;

namespace Aplicacao
{
    public interface ISecaoProdutoAplicacao : IBaseAplicacao<SecaoProduto>
    {
        IEnumerable<SecaoProdutoViewModel> Listar();

        IEnumerable<SecaoProdutoViewModel> Listar(int idDepartamento);
    }

    public class SecaoProdutoAplicacao : BaseAplicacao<SecaoProduto, ISecaoProdutoServico>, ISecaoProdutoAplicacao
    {
        private readonly ISecaoProdutoServico _servico;
        public SecaoProdutoAplicacao(ISecaoProdutoServico servico)
        {
            _servico = servico;
        }

        public new void Salvar(SecaoProduto entity)
        {
            var grupoRetorno = BuscarPorId(entity.Id) ?? entity;

            grupoRetorno.Id = entity.Id;
            grupoRetorno.Nome = entity.Nome;

            if (string.IsNullOrEmpty(grupoRetorno.Nome)) throw new Exception("Campos preenchidos incorretamente!");

            Servico.Salvar(grupoRetorno);
        }

        public IEnumerable<SecaoProdutoViewModel> Listar()
        {
            return _servico.Buscar().Select(x => new SecaoProdutoViewModel(x)).ToList();
        }

        public IEnumerable<SecaoProdutoViewModel> Listar(int idDepartamento)
        {
            return _servico.Buscar().Where(x => x.Departamento.Id == idDepartamento).Select(x => new SecaoProdutoViewModel(x)).ToList();
        }
    }
}