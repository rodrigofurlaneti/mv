using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoMap : ClassMap<Pedido>
    {
        public PedidoMap()
        {
            Table("Pedido");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Motivo).Column("Motivo");
            Map(x => x.CodigoPcSist).Column("CodigoPcSist");

            Map(x => x.CodEstabelecimentoInfox).Column("CodEstabelecimentoInfox").Nullable();
            Map(x => x.NSURedeRXInfox).Column("NSURedeRXInfox").Nullable();
            Map(x => x.Valor).Column("Valor").Precision(19).Scale(5);

            References(x => x.ListaCompra).Column("ListaCompra");

            References(x => x.Usuario).Column("Usuario").Cascade.None();
            References(x => x.Cartao).Column("Cartao").Cascade.None();
            References(x => x.Endereco).Column("Endereco").Cascade.None();

            References(x => x.Agendamento).Column("Agendamento").Cascade.SaveUpdate();

            References(x => x.QrCode).Column("QrCode");

            References(x => x.Loja).Column("Loja").Cascade.None();

            HasMany(x => x.ListaHistorico)
                .Table("HistoricoPedido")
                .KeyColumn("Pedido_id")
                .Cascade.All();

            References(x => x.AvaliacaoPedido).Column("AvaliacaoPedido").Cascade.All();
        }
    }
}