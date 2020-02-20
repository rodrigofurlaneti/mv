using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IDepartamentoProdutoServico : IBaseServico<DepartamentoProduto>
    {
        IList<DepartamentoProduto> BuscarPorLoja(int loja);
    }

    public class DepartamentoProdutoServico : BaseServico<DepartamentoProduto, IDepartamentoProdutoRepositorio>, IDepartamentoProdutoServico
    {
        private readonly IDepartamentoProdutoRepositorio _departamentoProdutoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public DepartamentoProdutoServico(
            IDepartamentoProdutoRepositorio departamentoProdutoRepositorio,
            IProdutoRepositorio produtoRepositorio)
        {
            _departamentoProdutoRepositorio = departamentoProdutoRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        public IList<DepartamentoProduto> BuscarPorLoja(int loja)
        {
            return _produtoRepositorio.BuscarDepartamentoPorLoja(loja);
        }
    }
}