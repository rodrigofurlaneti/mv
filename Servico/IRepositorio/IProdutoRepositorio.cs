using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IProdutoRepositorio : IRepository<Produto>
    {
        IList<DepartamentoProduto> BuscarDepartamentoPorLoja(int loja);
        IList<SecaoProduto> BuscarSecaoPorDepartamentoELoja(int loja, int departamento);
        List<Produto> BucarUltimoIdProduto();
    }
}
