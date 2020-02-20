using Entidade;

namespace Aplicacao.ViewModels
{
    public class SubGrupoProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public GrupoProdutoViewModel Grupo { get; set; }

        public SubGrupoProdutoViewModel()
        {

        }

        public SubGrupoProdutoViewModel(SubGrupoProduto entidade)
        {
            Id = entidade?.Id ?? 0;
            Nome = entidade?.Nome;
            Grupo = AutoMapper.Mapper.Map<GrupoProduto, GrupoProdutoViewModel>(entidade?.Grupo);
        }

        public SubGrupoProduto ToEntity() => new SubGrupoProduto()
        {
            Id = this.Id,
            Nome = this.Nome,
            Grupo = AutoMapper.Mapper.Map<GrupoProdutoViewModel, GrupoProduto>(this?.Grupo)
        };
    }
}