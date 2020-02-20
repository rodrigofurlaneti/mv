using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PdvMap : ClassMap<Pdv>
    {
        public PdvMap()
        {
            Table("Pdv");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Codigo).Column("Codigo");
            Map(x => x.Descricao).Column("Descricao");

            References(x => x.Loja).Column("Loja").Not.Nullable();

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}