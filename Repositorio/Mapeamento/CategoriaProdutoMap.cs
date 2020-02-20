using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CategoriaProdutoMap : ClassMap<CategoriaProduto>
    {
        public CategoriaProdutoMap()
        {
            Table("CategoriaProduto");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
        }
    }
}
