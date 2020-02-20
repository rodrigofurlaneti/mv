using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICategoriaProdutoServico : IBaseServico<CategoriaProduto>
    {
	    IList<CategoriaProduto> GetCategorias();
	    IList<CategoriaProduto> GetCategoriasPorSecao(int secaoId);
    }

    public class CategoriaProdutoServico : BaseServico<CategoriaProduto, ICategoriaProdutoRepositorio>, ICategoriaProdutoServico
	{
		private readonly ICategoriaProdutoRepositorio _categoriaProdutoRepositorio;
		private readonly ISecaoProdutoServico _secaoProdutoServico;

		public CategoriaProdutoServico(ICategoriaProdutoRepositorio categoriaProdutoRepositorio, ISecaoProdutoServico secaoProdutoServico)
		{
			_categoriaProdutoRepositorio = categoriaProdutoRepositorio;
			_secaoProdutoServico = secaoProdutoServico;
		}

		public IList<CategoriaProduto> GetCategorias()
		{
			return _categoriaProdutoRepositorio.List();
		}

		public IList<CategoriaProduto> GetCategoriasPorSecao(int secaoId)
		{
			var lista = _secaoProdutoServico.BuscarPor(s => s.Id == secaoId);

			return lista != null ? 
				lista.Select(s => s.Departamento.CategoriaProduto).Distinct().ToList() : 
				new List<CategoriaProduto>();
		}
	}
}
