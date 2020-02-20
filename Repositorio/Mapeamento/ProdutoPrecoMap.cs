using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ProdutoPrecoMap : ClassMap<ProdutoPreco>
    {
        public ProdutoPrecoMap()
        {
            Table("ProdutoPreco");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(8).Scale(2);
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.ValorDesconto).Column("ValorDesconto").Length(10).Precision(8).Scale(2);
            Map(x => x.InicioVigencia).Column("InicioVigencia");
            Map(x => x.FimVigencia).Column("FimVigencia");
            Map(x => x.Status).Column("Status").Default("0");
            Map(x => x.CodigoDesconto).Column("CodigoDesconto");
            Map(x => x.LinkDesconto).Column("LinkDesconto");

            References(x => x.Produto).Columns("Produto").Not.Nullable().Cascade.None();
            References(x => x.Loja).Column("Loja").Nullable().Cascade.None(); ;
            References(x => x.Fornecedor).Column("Fornecedor").Nullable().Cascade.None(); 
        }
    }
}