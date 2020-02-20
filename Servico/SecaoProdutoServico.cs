using System.Collections.Generic;
using System.Linq;
using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ISecaoProdutoServico : IBaseServico<SecaoProduto>
    {
	    IList<SecaoProduto> GetSecoesPorCategoria(int categoriaId);
        IList<SecaoProduto> GetSecoesPorDepartamento(int departamento);
        IList<SecaoProduto> BuscarSecaoPorDepartamentoELoja(int loja, int departamento);
    }

    public class SecaoProdutoServico : BaseServico<SecaoProduto, ISecaoProdutoRepositorio>, ISecaoProdutoServico
    {
        private readonly ISecaoProdutoRepositorio _secaoProdutoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public SecaoProdutoServico(
            ISecaoProdutoRepositorio secaoProdutoRepositorio,
            IProdutoRepositorio produtoRepositorio)
        {
            _secaoProdutoRepositorio = secaoProdutoRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        private static void Validar(SecaoProduto entity)
        {
            if (string.IsNullOrEmpty(entity.Nome))
                throw new BusinessRuleException("Informe o Nome!");
            if (entity.Departamento.Id == 0)
                throw new BusinessRuleException("Selecione um departamento!");
        }

        public new void Salvar(SecaoProduto secao)
        {
            Validar(secao);

            Repositorio.Save(secao);
        }

	    public IList<SecaoProduto> GetSecoesPorCategoria(int categoriaId)
	    {
		    return Repositorio.ListBy(s => s.Departamento.CategoriaProduto.Id == categoriaId);
	    }

        public IList<SecaoProduto> GetSecoesPorDepartamento(int departamento)
        {
            return Repositorio.ListBy(s => s.Departamento.Id == departamento)?.ToList() ?? new List<SecaoProduto>();
        }

        public IList<SecaoProduto> BuscarSecaoPorDepartamentoELoja(int loja, int departamento)
        {
            return _produtoRepositorio.BuscarSecaoPorDepartamentoELoja(loja, departamento);
        }
    }
}