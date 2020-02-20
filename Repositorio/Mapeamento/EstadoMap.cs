using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EstadoMap : ClassMap<Estado>
    {
        public EstadoMap()
        {
            Table("Estado");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.Sigla).Column("Sigla");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            References(x => x.Pais).Column("Pais").Not.LazyLoad();
        }
    }
}
