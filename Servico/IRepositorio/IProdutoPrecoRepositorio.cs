using System.Collections.Generic;
using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Uteis;

namespace Dominio.IRepositorio
{
    public interface IProdutoPrecoRepositorio : IRepository<ProdutoPreco>
    {
        ProdutoPreco GetByLojaProduto(int lojaId, int produtoId);
        ProdutoPreco GetByFornecedorProduto(int fornecedorId, int produtoId);
        IList<ProdutoPreco> GetByFornecedorProduto(int fornecedorId);
        IList<ProdutoPreco> ListProdutosVigentes();
        ProdutoPreco GetByLojaProdutoCodigoDeBarras(int lojaId, string codigoDeBarras);
    }
}
