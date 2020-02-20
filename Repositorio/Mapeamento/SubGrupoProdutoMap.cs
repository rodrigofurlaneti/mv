using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class SubGrupoProdutoMap : ClassMap<SubGrupoProduto>
    {
        public SubGrupoProdutoMap()
        {
            Table("SubGrupoProduto");
            LazyLoad();

            Id(x => x.Id, "Id").GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");

            References(x => x.Grupo).Column("Grupo");
        }
    }
}
