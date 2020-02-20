using System.Collections.Generic;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public string CodigoBarras { get; set; }
        public List<InformacaoProduto> Informacoes { get; set; }
        public List<int> Lojas { get; set; }
        public int CodigoPcSist { get; set; }
        public string CodigoAuxiliarPcSist { get; set; }
        public DepartamentoProdutoViewModel DepartamentoProduto { get; set; }

        public ProdutoViewModel()
        {

        }

        public ProdutoViewModel(Produto entidade)
        {
            Id = entidade?.Id ?? 0;
            Nome = entidade?.Nome;
            DepartamentoProduto = AutoMapper.Mapper.Map<DepartamentoProduto, DepartamentoProdutoViewModel>(entidade?.DepartamentoProduto);
        }

        public Produto ToEntity() => new Produto()
        {
            Id = this.Id,
            Nome = this.Nome,
            DepartamentoProduto = AutoMapper.Mapper.Map<DepartamentoProdutoViewModel, DepartamentoProduto>(this?.DepartamentoProduto)
        };
    }
}