using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TerminalCobrancaLojaMap : ClassMap<TerminalCobrancaLoja>
    {
        public TerminalCobrancaLojaMap()
        {
            Table("TerminalCobrancaLoja");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Loja).Column("Loja_id").Cascade.None();
            References(x => x.Usuario).Cascade.None();
            References(x => x.Terminal).Column("TerminalCobranca_id").Cascade.All();
        }
    }
}
