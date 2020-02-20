using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public interface IDescontoGlobalServico : IBaseServico<DescontoGlobal>
    {
        void Salvar(IList<DescontoGlobal> descontos);

        void ExcluirPorProduto(List<int> produtosPreco);
    }

    public class DescontoGlobalServico : BaseServico<DescontoGlobal, IDescontoGlobalRepositorio>, IDescontoGlobalServico
    {
        private readonly IDescontoGlobalRepositorio _descontoGlobalRepositorio;

        public DescontoGlobalServico(IDescontoGlobalRepositorio descontoGlobalRepositorio)
        {
            _descontoGlobalRepositorio = descontoGlobalRepositorio;
        }

        public void Salvar(IList<DescontoGlobal> descontos)
        {
            _descontoGlobalRepositorio.Save(descontos);
        }

        public void ExcluirPorProduto(List<int> produtosPreco)
        {
            StringBuilder sb = new StringBuilder();

            string prods = String.Join(",", produtosPreco);

            string consulta = $"delete from DescontoGlobal where ProdutoPreco IN( {prods})";

            _descontoGlobalRepositorio.DeleteByQuery(consulta);
        }
    }
}