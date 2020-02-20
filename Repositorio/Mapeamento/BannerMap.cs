using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class BannerMap: ClassMap<Banner>
    {
        public BannerMap()
        {
            Table("Banner");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.URL).Column("URL");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.TipoBanner).Column("TipoBanner");
            Map(x => x.DataInicio).Column("DataInicio");
            Map(x => x.DataFim).Column("DataFim");
        }
    }
}