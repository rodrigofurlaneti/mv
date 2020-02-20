using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoCartaoMap: ClassMap<TipoCartao>
    {
        public TipoCartaoMap()
        {
            Table("TipoCartao");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.AdiantamentoSalarial).Column("AdiantamentoSalarial");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}