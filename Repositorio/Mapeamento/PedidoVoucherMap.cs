using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoVoucherMap : ClassMap<PedidoVoucher>
    {
        public PedidoVoucherMap()
        {
            Table("PedidoVoucher");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.ValorVoucher).Column("ValorVoucher").Precision(19).Scale(5);

            References(x => x.Usuario).Column("Usuario").Cascade.None();
            References(x => x.QrCode).Column("QrCode");
            References(x => x.Loja).Column("Loja").Cascade.None();
            References(x => x.Fornecedor).Column("Fornecedor").Cascade.None();
            References(x => x.ProdutoPreco).Column("ProdutoPreco").Cascade.None();

            HasMany(x => x.ListaHistorico)
                .Table("HistoricoPedidoVoucher")
                .KeyColumn("PedidoVoucher")
                .Cascade.All();
        }
    }
}