using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DescontoPessoaMap : ClassMap<DescontoPessoa>
    {
        public DescontoPessoaMap()
        {
            Table("DescontoPessoa");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");

            References(x => x.Pessoa).Column("Pessoa").Cascade.None();
            References(x => x.Desconto).Column("Desconto").Cascade.None();
            References(x => x.ProdutoPreco).Column("ProdutoPreco").Cascade.None();
        }
    }
}
