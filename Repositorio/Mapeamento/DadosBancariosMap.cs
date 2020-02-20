using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DadosBancarioMap : ClassMap<DadosBancario>
    {
        public DadosBancarioMap()
        {
            Table("DadosBancario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Agencia).Column("Agencia");
            Map(x => x.DigitoAgencia).Column("DigitoAgencia");
            Map(x => x.Conta).Column("Conta");
            Map(x => x.DigitoConta).Column("DigitoConta");
            Map(x => x.DocumentoTitular).Column("DocumentoTitular").Nullable();
            Map(x => x.NomeTitular).Column("NomeTitular").Nullable();
            
            References(x => x.Banco).Column("Banco").Cascade.None();
        }
    }
}