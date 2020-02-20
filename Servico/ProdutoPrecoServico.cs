using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IProdutoPrecoServico : IBaseServico<ProdutoPreco>
    {
        ProdutoPreco GetByLojaProduto(int lojaId, int produtoId);
        ProdutoPreco GetByFornecedorProduto(int fornecedorId, int produtoId);
        IList<ProdutoPreco> GetByFornecedorProduto(int fornecedorId);
        IList<ProdutoPreco> ListProdutosVigentes();
    }

    public class ProdutoPrecoServico : BaseServico<ProdutoPreco, IProdutoPrecoRepositorio>, IProdutoPrecoServico
    {
        public ProdutoPreco GetByLojaProduto(int lojaId, int produtoId)
        {
            return Repositorio.ToCast<IProdutoPrecoRepositorio>().GetByLojaProduto(lojaId, produtoId);
        }

        public ProdutoPreco GetByFornecedorProduto(int fornecedorId, int produtoId)
        {
            return Repositorio.ToCast<IProdutoPrecoRepositorio>().GetByFornecedorProduto(fornecedorId, produtoId);
        }

        public IList<ProdutoPreco> GetByFornecedorProduto(int fornecedorId)
        {
            var produtosPreco = Repositorio.ToCast<IProdutoPrecoRepositorio>().GetByFornecedorProduto(fornecedorId);
            if (produtosPreco == null || !produtosPreco.Any())
                return produtosPreco;

            var produtos = produtosPreco.Select(x => x.Produto.Codigo).Distinct().ToList();
            return produtos.Select(item => produtosPreco.Where(x => x.Produto.Codigo == item).OrderBy(x => x.FimVigencia).FirstOrDefault()).ToList();
        }

        public IList<ProdutoPreco> ListProdutosVigentes()
        {
            var produtosPreco = Repositorio.ToCast<IProdutoPrecoRepositorio>().ListProdutosVigentes();
            if (produtosPreco == null || !produtosPreco.Any())
                return produtosPreco;

            var produtos = produtosPreco.Select(x => x.Produto.Codigo).Distinct().ToList();
            return produtos.Select(item => produtosPreco.Where(x => x.Produto.Codigo == item).OrderBy(x => x.FimVigencia).FirstOrDefault()).ToList();
        }
    }
}