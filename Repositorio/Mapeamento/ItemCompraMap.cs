using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ItemCompraMap : ClassMap<ItemCompra>
    {
        public ItemCompraMap()
        {
            Table("ItemCompra");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.Quantidade).Column("Quantidade");
            Map(x => x.Total).Column("Total");

            References(x => x.ListaCompra).Column("ListaCompra").Cascade.None();
            References(x => x.Produto).Columns("Produto").Cascade.None();
            References(x => x.Preco).Column("Preco").Cascade.None();

            References(x => x.Agendamento).Column("Agendamento").Cascade.SaveUpdate();
            
            References(x => x.Veiculo).Column("Veiculo").Cascade.None();

            References(x => x.PlanoVenda).Column("PlanoVenda").Cascade.None();

            References(x => x.Beneficiario).Column("Beneficiario").Cascade.None();
        }
    }
}
