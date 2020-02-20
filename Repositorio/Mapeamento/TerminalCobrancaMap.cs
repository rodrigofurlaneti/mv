using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TerminalCobrancaMap : ClassMap<TerminalCobranca>
    {
        public TerminalCobrancaMap()
        {
            Table("TerminalCobranca");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DataInsercao).Not.Update();
            Map(x => x.EnderecoIP);
            Map(x => x.EnderecoMAC);
            Map(x => x.Modelo);
            Map(x => x.NumeroSerial);
            Map(x => x.TipoTerminal);

            Map(x => x.Maquininha).Column("Maquininha");
            Map(x => x.NomeGerenciadora).Column("NomeGerenciadora");
            Map(x => x.SoftwareHouse).Column("SoftwareHouse");
            Map(x => x.TaxaCredito).Column("TaxaCredito").Length(10).Precision(8).Scale(2);
            Map(x => x.TaxaDebito).Column("TaxaDebito").Length(10).Precision(8).Scale(2);
        }
    }
}
