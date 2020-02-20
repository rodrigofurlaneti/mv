using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class PcEstMap : ClassMap<PcEst>
    {
        public PcEstMap()
        {
            Table("PCEST");
            LazyLoad();

            Id().GeneratedBy.Assigned();

            Map(x => x.CodFilial).Column("CodFilial");
            Map(x => x.CodProd).Column("CodProd");
            Map(x => x.QtEst).Column("QtEst");
            Map(x => x.CustoReal).Column("CustoReal");
            Map(x => x.DtUltEnt).Column("DtUltEnt");
            Map(x => x.DtUltSaida).Column("DtUltSaida");
           
        }
    }
}
