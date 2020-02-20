using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class AvaliacaoPedidoMap : ClassMap<AvaliacaoPedido>
    {
        public AvaliacaoPedidoMap()
        {
            Table("AvaliacaoPedido");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Comentario).Column("Comentario");
            Map(x => x.NotaPedido).Column("NotaPedido");
            Map(x => x.NotaAplicativo).Column("NotaAplicativo");
            Map(x => x.ItensDeAcordoComAnuncio).Column("ItensDeAcordoComAnuncio");

            HasMany(x => x.AvaliacaoItensPedido)
                .Table("AvaliacaoItensPedido")
                .KeyColumn("AvaliacaoPedido")
                .Component(m =>
                {
                    m.References(x => x.ItemCompra).Column("ItemCompra").Cascade.None();
                    m.Map(x => x.ItemDeAcordoComAnuncio).Column("ItemDeAcordoComAnuncio");
                    m.Map(x => x.Nota).Column("Nota");
                    m.Map(x => x.Comentario).Column("Comentario");

                }).Cascade.All();
        }
    }
}