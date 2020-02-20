using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class PedidoMap : ClassMap<Pedido>
    {
        public PedidoMap()
        {
            Table("PCPEDCFV");
            LazyLoad();

            Id(x => x.CodigoPedidoRCA).Column("NumPedRca").GeneratedBy.Assigned();
            Map(x => x.Importado).Column("Importado");
            Map(x => x.CodigoRCA).Column("CodUsur");
            Map(x => x.DocumentoCliente).Column("CGCCli");
            Map(x => x.DataAbertura).Column("DTABERTURAPEDPALM");
            Map(x => x.DataFechamento).Column("DTFECHAMENTOPEDPALM");
            Map(x => x.CodigoFilial).Column("CodFilial");
            Map(x => x.CodigoCobranca).Column("CodCob");
            Map(x => x.CodigoPlanoPagamento).Column("CodPlPag");
            Map(x => x.ValorDesconto).Column("VlDescAbatimento");
            Map(x => x.CondicaoVenda).Column("CondVenda");
            Map(x => x.Origem).Column("OrigemPed");
        }
    }
}