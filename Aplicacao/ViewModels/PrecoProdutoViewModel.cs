using System;
using System.Collections.Generic;
using System.Globalization;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ProdutoPrecoViewModel
    {
        public int Id { get; set; }

        public string ProdutoId { get; set; }

        public ProdutoViewModel Produto { get; set; }

        public string FornecedorId { get; set; }

        public List<int> Fornecedores { get; set; }
	    
	    public List<int> Lojas { get; set; }

		public FornecedorViewModel Fornecedor { get; set; }

        public string Valor { get; set; }

        public string ValorDesconto { get; set; }

        public string InicioVigencia { get; set; }

        public string FimVigencia { get; set; }

        public DateTime DataInsercao { get; set; }

        public LojaViewModel Loja { get; set; }

        public ProdutoPrecoViewModel()
        {

        }

        public ProdutoPrecoViewModel(bool defaultDates)
        {
            InicioVigencia = DateTime.Now.ToString("dd/MM/yyyy");
            FimVigencia = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
        }

        public ProdutoPrecoViewModel(ProdutoPreco entidade)
        {
            Id = entidade?.Id ?? 0;
            DataInsercao = entidade?.DataInsercao ?? DateTime.Now;
            FimVigencia = (entidade?.FimVigencia ?? DateTime.Now).ToString("dd/MM/yyyy");
            Fornecedor = new FornecedorViewModel(entidade?.Fornecedor);
            Loja = new LojaViewModel(entidade?.Loja);
            FornecedorId = Fornecedor.Id.ToString();
            InicioVigencia = (entidade?.InicioVigencia ?? DateTime.Now).ToString("dd/MM/yyyy");
            Produto = new ProdutoViewModel(entidade?.Produto);
            ProdutoId = Produto.Id.ToString();
            Valor = (entidade?.Valor.ToString("c", new CultureInfo("pt-BR")) ?? "0,00").Replace("R$", "");
            ValorDesconto = (entidade?.ValorDesconto.ToString("c", new CultureInfo("pt-BR")) ?? "0,00").Replace("R$", "");
        }
    }
}