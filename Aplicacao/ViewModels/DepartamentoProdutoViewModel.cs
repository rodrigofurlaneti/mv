using Entidade;

namespace Aplicacao.ViewModels
{
    public class DepartamentoProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public DepartamentoProdutoViewModel()
        {

        }

        public DepartamentoProdutoViewModel(DepartamentoProduto grupo)
        {
            Id = grupo?.Id ?? 0;
            Nome = grupo?.Nome;
        }

        public DepartamentoProduto ToEntity() => new DepartamentoProduto()
        {
            Id = this.Id,
            Nome = this.Nome
        };
    }
}