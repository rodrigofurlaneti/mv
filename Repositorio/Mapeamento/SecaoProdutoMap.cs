using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class SecaoProdutoMap : ClassMap<SecaoProduto>
    {
        public SecaoProdutoMap()
        {
            Table("SecaoProduto");
            LazyLoad();

            Id(x => x.Id, "Id").GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");

            References(x => x.Departamento).Column("Departamento");
        }
    }
}
