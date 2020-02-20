using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class ProdutoMap : ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Table("Produto");
            LazyLoad();

            //Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Id().GeneratedBy.Assigned();

            Map(x => x.Codigo).Column("Codigo");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.CodigoBarras).Column("CodigoBarras");
            
            HasMany(x => x.Informacoes);
        }
    }
}