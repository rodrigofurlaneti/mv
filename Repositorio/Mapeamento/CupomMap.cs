using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CupomMap : ClassMap<Cupom>
    {
        public CupomMap()
        {
            Table("Cupom");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.CodigoCupom).Column("Codigo");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.TipoDesconto).Column("TipoDesconto");
            Map(x => x.ValidadeFinal).Column("ValidadeFinal");
            Map(x => x.ValorCupom).Column("ValorCupom");
        }
    }
}
