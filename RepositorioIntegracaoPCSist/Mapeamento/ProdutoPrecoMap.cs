using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class ProdutoPrecoMap : ClassMap<ProdutoPreco>
    {
        public ProdutoPrecoMap()
        {
            Table("ProdutoPreco");
            LazyLoad();

            //Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Id().GeneratedBy.Assigned();

            Map(x => x.Valor).Column("Valor");
            //Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            //
            References(x => x.Produto).Column("Produto").Not.Nullable();
            References(x => x.Loja).Column("Loja").Not.Nullable();
        }
    }
}