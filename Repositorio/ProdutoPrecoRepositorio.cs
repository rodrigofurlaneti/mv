using System;
using System.Collections.Generic;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using NHibernate.Criterion;
using Repositorio.Base;

namespace Repositorio
{
    public class ProdutoPrecoRepositorio : NHibRepository<ProdutoPreco>, IProdutoPrecoRepositorio
    {
        public ProdutoPrecoRepositorio(NHibContext context)
            : base(context)
        {
        }

        public ProdutoPreco GetByLojaProdutoCodigoDeBarras(int lojaId, string codigoDeBarras)
        {
            return FirstBy(x => x.Valor > 0 && x.Loja.Id.Equals(lojaId) && x.Produto.CodigoBarras.Equals(codigoDeBarras) && x.InicioVigencia <= DateTime.Now && x.FimVigencia >= DateTime.Now);
        }

        public ProdutoPreco GetByLojaProduto(int lojaId, int produtoId)
        {
            return FirstBy(x => x.Valor > 0 && x.Loja.Id.Equals(lojaId) && x.Produto.Id.Equals(produtoId) && x.InicioVigencia <= DateTime.Now && x.FimVigencia >= DateTime.Now);
        }

        public ProdutoPreco GetByFornecedorProduto(int fornecedorId, int produtoId)
        {
            return FirstBy(x => x.Valor > 0 && x.Fornecedor.Id.Equals(fornecedorId) && x.Produto.Id.Equals(produtoId) && x.InicioVigencia <= DateTime.Now && x.FimVigencia >= DateTime.Now);
        }

        public IList<ProdutoPreco> GetByFornecedorProduto(int fornecedorId)
        {
            return ListBy(x => x.Valor > 0 && x.Fornecedor.Id.Equals(fornecedorId) && x.InicioVigencia <= DateTime.Now && x.FimVigencia >= DateTime.Now);
        }

        public IList<ProdutoPreco> ListProdutosVigentes()
        {
            return ListBy(x => x.Valor > 0 && x.InicioVigencia <= DateTime.Now && x.FimVigencia >= DateTime.Now);
        }
    }
}