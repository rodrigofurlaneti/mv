using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IDescontoPessoaAplicacao : IBaseAplicacao<DescontoPessoa>
    {
        void Salvar(List<int> pessoas, Desconto desconto, List<int> produtosPreco);
    }

    public class DescontoPessoaAplicacao : BaseAplicacao<DescontoPessoa, IDescontoPessoaServico>, IDescontoPessoaAplicacao
    {
        private readonly IDescontoPessoaServico _descontoPessoaServico;

        public DescontoPessoaAplicacao(IDescontoPessoaServico descontoPessoaServico)
        {
            _descontoPessoaServico = descontoPessoaServico;
        }

        public new void Salvar(DescontoPessoa entity)
        {
            var descontoPessoaRetorno = BuscarPorId(entity.Id) ?? entity;
            descontoPessoaRetorno.Desconto = entity.Desconto;
            descontoPessoaRetorno.Pessoa = entity.Pessoa;
            descontoPessoaRetorno.ProdutoPreco = entity.ProdutoPreco;
            descontoPessoaRetorno.DataInsercao = DateTime.Now;

            _descontoPessoaServico.Salvar(descontoPessoaRetorno);
        }

        public void Salvar(List<int> pessoas, Desconto desconto, List<int> produtosPreco)
        {
            var descontos = new List<DescontoPessoa>();

            pessoas.ForEach(p =>
            {
                produtosPreco.ForEach(pp =>
                {
                    descontos.Add(new DescontoPessoa
                    {
                        Desconto = desconto,
                        Pessoa = new Pessoa { Id = p },
                        ProdutoPreco = new ProdutoPreco { Id = pp }
                    });
                });
            });

            if (!descontos.Any()) return;

            _descontoPessoaServico.ExcluirPorDesconto(desconto.Id);
            _descontoPessoaServico.Salvar(descontos);
        }
    }
}