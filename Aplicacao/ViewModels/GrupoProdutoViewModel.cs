using System.ComponentModel.DataAnnotations;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class GrupoProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [Display(Name = "Secao")]
        public SecaoProdutoViewModel Secao { get; set; }

        public GrupoProdutoViewModel()
        {

        }

        public GrupoProdutoViewModel(GrupoProduto grupo)
        {
            Id = grupo?.Id ?? 0;
            Nome = grupo?.Nome;
            Secao = AutoMapper.Mapper.Map<SecaoProduto, SecaoProdutoViewModel>(grupo?.Secao);
        }

        public GrupoProduto ToEntity() => new GrupoProduto()
        {
            Id = this.Id,
            Nome = this.Nome,
            Secao = AutoMapper.Mapper.Map<SecaoProdutoViewModel, SecaoProduto>(this.Secao)
        };
    }
}