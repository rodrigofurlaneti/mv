using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabloideMap: ClassMap<Tabloide>
    {
        public TabloideMap()
        {
            Table("Tabloide");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.URL).Column("URL");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}