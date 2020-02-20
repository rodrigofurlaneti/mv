using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class ItemCompraMap : ClassMap<ItemCompra>
    {
        public ItemCompraMap()
        {
            Table("PCPEDIFV");
            LazyLoad();

            //CompositeId()
            //    .KeyProperty(x => x.CodigoPedigoRCA, "NumPedRCA")
            //    .KeyProperty(x => x.CodigoProduto, "CodProd")
            //    .KeyProperty(x => x.NumeroSequencia, "NumSeq");

            Id(x => x.Id).Column("OBSERVACAO_PC").GeneratedBy.Assigned();
            Map(x => x.CodigoPedigoRCA).Column("NumPedRCA");
            Map(x => x.DocumentoCliente).Column("CGCCLI");
            Map(x => x.CodigoRCA).Column("CodUsur");
            Map(x => x.DataAberturaPedido).Column("DtAberturaPedPalm");
            Map(x => x.CodigoProduto).Column("CodProd");
            Map(x => x.Quantidade).Column("Qt");
            Map(x => x.PrecoUnidade).Column("PVenda");
            Map(x => x.NumeroSequencia).Column("NumSeq");
            Map(x => x.PercentualDescontoEDI).Column("PercDescEDI");
            Map(x => x.PercentualDescontoBoleto).Column("PerDescBoleto");
            Map(x => x.CodigoAuxiliar).Column("CODAUXILIAR");
        }
    }
}
