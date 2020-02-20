using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IDescontoGlobalAplicacao : IBaseAplicacao<DescontoGlobal>
    {
        void Salvar(Desconto desconto, List<int> produtosPreco);
        void Excluir(int id);
    }

    public class DescontoGlobalAplicacao : BaseAplicacao<DescontoGlobal, IDescontoGlobalServico>, IDescontoGlobalAplicacao
    {
        private readonly IDescontoGlobalServico _descontoGlobalServico;
        public DescontoGlobalAplicacao(IDescontoGlobalServico descontoGlobalServico)
        {
            _descontoGlobalServico = descontoGlobalServico;
        }


        public void Excluir(int id)
        {
            //Remove item
            _descontoGlobalServico.ExcluirPorId(id);
        }

        public new void Salvar(DescontoGlobal entity)
        {

            var descontoRetorno = BuscarPorId(entity.Id) ?? entity;
            descontoRetorno.Desconto = entity.Desconto;
            descontoRetorno.ProdutoPreco = entity.ProdutoPreco;
            descontoRetorno.DataInsercao = DateTime.Now;
            _descontoGlobalServico.Salvar(descontoRetorno);
        }

        public void Salvar(Desconto desconto, List<int> produtosPreco)
        {
            var descontos = new List<DescontoGlobal>();

            produtosPreco.ForEach(pp =>
            {
                descontos.Add(new DescontoGlobal
                {
                    Desconto = desconto,
                    ProdutoPreco = new ProdutoPreco { Id = pp }
                });
            });


            if (!descontos.Any()) return;

            _descontoGlobalServico.ExcluirPorProduto(produtosPreco);

            _descontoGlobalServico.Salvar(descontos);
        }

    }
}