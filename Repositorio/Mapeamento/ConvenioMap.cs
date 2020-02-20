using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ConvenioMap : ClassMap<Convenio>
    {
        public ConvenioMap()
        {
            Table("Convenio");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Cnpj).Column("Cnpj");
            Map(x => x.RazaoSocial).Column("RazaoSocial");
            Map(x => x.Status).Column("Status");

            References(x => x.Endereco).Column("Endereco").Nullable().Cascade.All();
        }
    }
}