using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class GrupoProdutoMap : ClassMap<GrupoProduto>
    {
        public GrupoProdutoMap()
        {
            Table("GrupoProduto");
            LazyLoad();

            Id(x => x.Id, "Id").GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");

            References(x => x.Secao).Column("Secao");
        }
    }
}
