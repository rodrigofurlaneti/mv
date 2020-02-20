using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class AtendimentoProdutoMap: ClassMap<AtendimentoProduto>
    {
        public AtendimentoProdutoMap()
        {
            Table("AtendimentoProduto");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(8).Scale(2);
            Map(x => x.Descricao).Column("Descricao").Length(1000);
            
            References(x => x.Produto).Columns("Produto").Not.Nullable();
        }
    }
}