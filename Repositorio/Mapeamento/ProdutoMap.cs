using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ProdutoMap : ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Table("Produto");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.CodigoBarras).Column("CodigoBarras");

            Map(x => x.Nome).Column("Nome");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Codigo).Column("Codigo");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(p => p.DepartamentoProduto).Columns("DepartamentoProduto");

            HasMany(x => x.Informacoes).Inverse().Cascade.AllDeleteOrphan();

            HasMany(x => x.Atendimentos).Inverse();
        }
    }
}