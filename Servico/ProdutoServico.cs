using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using NHibernate.Util;

namespace Dominio
{
    public interface IProdutoServico : IBaseServico<Produto>
    {
        IList<Produto> GetProdutosPorDepartamento(int departamentoId, int lojaId);
        IList<Produto> GetProdutosPorCategoria(int categoriaId, int lojaId);
        IList<DepartamentoProduto> BuscarDepartamentoPorLoja(int loja);
        IList<SecaoProduto> BuscarSecaoPorDepartamentoELoja(int loja, int departamento);
        IList<Produto> GetProdutos(int inicio, int quantidade);
        IList<Produto> GetProdutosPorCategoria(int categoriaId);
    }

    public class ProdutoServico : BaseServico<Produto, IProdutoRepositorio>, IProdutoServico
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoServico(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public IList<Produto> GetProdutosPorDepartamento(int departamentoId, int lojaId)
        {
            var lista = Repositorio.ListBy(p => p.DepartamentoProduto.Id == departamentoId && p.Lojas.Any(l => l.Id == lojaId));

            return FiltrarPreco(lista, lojaId);
        }

        public IList<Produto> GetProdutosPorCategoria(int categoriaId)
        {
            var lista = Repositorio.ListBy(p => p.DepartamentoProduto.CategoriaProduto.Id == categoriaId);

            return lista;
        }

        public IList<Produto> GetProdutosPorCategoria(int categoriaId, int lojaId)
	    {
		    var lista = Repositorio.ListBy(p => p.DepartamentoProduto.CategoriaProduto.Id == categoriaId && p.Lojas.Any(l => l.Id == lojaId));

		    return FiltrarPreco(lista, lojaId);
		}

		private IList<Produto> FiltrarPreco(IList<Produto> produtos, int lojaId)
	    {
		    produtos.ForEach(p => p.Precos = p.Precos.Where(pp => pp.Loja.Id == lojaId).ToList());

		    return produtos;
		}

        public IList<DepartamentoProduto> BuscarDepartamentoPorLoja(int loja)
        {
            return _produtoRepositorio.BuscarDepartamentoPorLoja(loja);
        }

        public IList<SecaoProduto> BuscarSecaoPorDepartamentoELoja(int loja, int departamento)
        {
            return _produtoRepositorio.BuscarSecaoPorDepartamentoELoja(loja, departamento);
        }

        public IList<Produto> GetProdutos(int inicio, int quantidade)
        {
            return Repositorio.List().Skip(inicio).Take(quantidade).ToList();
        }
    }
}
