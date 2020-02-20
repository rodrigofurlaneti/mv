using System;
using System.Collections.Generic;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class DescontoGlobalViewModel
    {
        public int Id { get; set; }
        public TipoCadastroDesconto TipoCadastroDesconto { get; set; }
        public int DescontoId { get; set; }
        public DescontoViewModel Desconto { get; set; }
        public int IdadeDe { get; set; }
        public int IdadeAte { get; set; }
        public Sexo Sexo { get; set; }
        public List<int> Cidades { get; set; }
        public List<int> Estados { get; set; }
        public List<int> ProdutosPreco { get; set; }
        public List<PessoaRetornoFiltroViewModel> Filtro { get; set; }

        public PessoaViewModel Pessoa { get; set; }

        public DescontoGlobalViewModel()
        {

        }

        public DescontoGlobalViewModel(DescontoGlobal desconto)
        {
            Id = desconto.Id;
            DescontoId = desconto.Id;
            Desconto = new DescontoViewModel(desconto?.Desconto);
        }

        public DescontoGlobalViewModel(DescontoPessoa desconto)
        {
            Id = desconto.Id;
            DescontoId = desconto.Id;
            Pessoa = new PessoaViewModel(desconto.Pessoa);
            Desconto = new DescontoViewModel(desconto?.Desconto);
        }

        public DescontoGlobal ToEntityGlobal()
        {
            return new DescontoGlobal()
            {
                Id = this.Id,
                DataInsercao = DateTime.Now,
                Desconto = new Desconto { Id = this.DescontoId }
            };
        }

        public DescontoPessoa ToEntityPessoa()
        {
            return new DescontoPessoa()
            {
                Id = this.Id,
                DataInsercao = DateTime.Now,
                Desconto = new Desconto { Id = this.DescontoId }
            };
        }
    }
}