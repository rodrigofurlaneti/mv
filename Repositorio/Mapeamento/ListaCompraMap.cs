using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ListaCompraMap : ClassMap<ListaCompra>
    {
        public ListaCompraMap()
        {
            Table("ListaCompra");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.SubTotal).Column("SubTotal").Precision(19).Scale(5);
            Map(x => x.Total).Column("Total").Precision(19).Scale(5);
            Map(x => x.Cupom).Column("Cupom");
            Map(x => x.ValorCupom).Column("ValorCupom").Precision(19).Scale(5);
            
            References(x => x.Usuario).Column("Usuario").Cascade.None();
            References(x => x.Loja).Column("Loja").Cascade.None();
            HasMany(x => x.Itens);
        }
    }
}
