using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class HistoricoPedidoVoucherMap : ClassMap<HistoricoPedidoVoucher>
    {
        public HistoricoPedidoVoucherMap()
        {
            Table("HistoricoPedidoVoucher");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").CustomSqlType("varchar(max)").Nullable();
            Map(x => x.StatusPedido).Column("StatusPedido").Not.Nullable();
            
            References(x => x.UsuarioLogado).Column("UsuarioLogado");
            References(x => x.Pedido).Column("Pedido");
        }
    }
}