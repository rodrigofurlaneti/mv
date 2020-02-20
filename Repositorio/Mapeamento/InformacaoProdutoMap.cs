using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class InformacaoProdutoMap: ClassMap<InformacaoProduto>
    {
        public InformacaoProdutoMap()
        {
            Table("InformacaoProduto");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Tipo).Column("Tipo");
            Map(x => x.Descricao).Column("Descricao").Length(1000);
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            References(x => x.Produto).Columns("Produto").Not.Nullable();
        }
    }
}