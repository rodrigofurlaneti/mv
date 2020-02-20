using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoLojaMap: ClassMap<TipoLoja>
    {
        public TipoLojaMap()
        {
            Table("TipoLoja");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}