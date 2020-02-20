using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DescontoMap : ClassMap<Desconto>
    {
        public DescontoMap()
        {
            Table("Desconto");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.TipoDesconto).Column("TipoDesconto");
            Map(x => x.ValorDesconto).Column("ValorDesconto");
            Map(x => x.Validade).Column("Validade");
            Map(x => x.LimiteDeCompra).Column("LimiteDeCompra").Default("0");
        }
    }
}
