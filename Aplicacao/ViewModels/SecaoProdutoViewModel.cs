using System.ComponentModel.DataAnnotations;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class SecaoProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [Display(Name="Departamento")]
        public DepartamentoProdutoViewModel Departamento { get; set; }

        public SecaoProdutoViewModel()
        {

        }

        public SecaoProdutoViewModel(SecaoProduto entidade)
        {
            Id = entidade?.Id ?? 0;
            Nome = entidade?.Nome;
            Departamento = AutoMapper.Mapper.Map<DepartamentoProduto, DepartamentoProdutoViewModel>(entidade?.Departamento);
        }

        public SecaoProduto ToEntity() => new SecaoProduto()
        {
            Id = this.Id,
            Nome = this.Nome,
            Departamento = AutoMapper.Mapper.Map<DepartamentoProdutoViewModel, DepartamentoProduto>(this.Departamento)
        };
    }
}