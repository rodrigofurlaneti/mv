using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DescontoGlobalMap : ClassMap<DescontoGlobal>
    {
        public DescontoGlobalMap()
        {
            Table("DescontoGlobal");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.ProdutoPreco).Column("ProdutoPreco").Cascade.None();
            References(x => x.Desconto).Column("Desconto").Cascade.None();
        }
    }
}