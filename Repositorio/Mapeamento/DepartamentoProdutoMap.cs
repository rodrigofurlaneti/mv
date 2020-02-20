using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DepartamentoProdutoMap : ClassMap<DepartamentoProduto>
    {
        public DepartamentoProdutoMap()
        {
            Table("DepartamentoProduto");
            LazyLoad();

            Id(x => x.Id, "Id").GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.LogoUpload).Column("LogoUpload");

            References(x => x.CategoriaProduto).Column("CategoriaProduto");
        }
    }
}
