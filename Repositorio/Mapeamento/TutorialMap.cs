using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TutorialMap: ClassMap<Tutorial>
    {
        public TutorialMap()
        {
            Table("Tutorial");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.URL).Column("URL");
            Map(x => x.Logado).Column("Logado");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}