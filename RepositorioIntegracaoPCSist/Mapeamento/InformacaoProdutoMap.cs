using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class InformacaoProdutoMap : ClassMap<InformacaoProduto>
    {
        public InformacaoProdutoMap()
        {
            Table("InformacaoProduto");
            LazyLoad();

            //Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Id().GeneratedBy.Assigned();

            Map(x => x.Tipo).Column("Tipo");
            Map(x => x.Descricao).Column("Descricao").Length(1000);
            //Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.Produto).Column("Produto").Not.Nullable();
        }
    }
}