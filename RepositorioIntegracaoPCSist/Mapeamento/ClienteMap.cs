using EntidadePcSist;
using FluentNHibernate.Mapping;

namespace RepositorioIntegracaoPCSist.Mapeamento
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("PCCLIENTFV");
            LazyLoad();
            
            Map(x => x.Importado).Column("IMPORTADO");
            Map(x => x.TipoOperacao).Column("TIPOOPERACAO");
            Id(x => x.CPFCNPJ).Column("CGCENT").GeneratedBy.Assigned();
            Map(x => x.RazaoSocial).Column("CLIENTE");
            Map(x => x.InscricaoEstadual).Column("IEENT");
            Map(x => x.Praca).Column("CODPRACA");
            Map(x => x.Telefone).Column("TELENT1");
            Map(x => x.Celular).Column("TELCOM");
            Map(x => x.Endereco).Column("ENDERCOM");
            Map(x => x.NumeroEndereco).Column("NUMEROCOM");
            Map(x => x.ComplementoEndereco).Column("COMPLEMENTOCOM");
            Map(x => x.BairroEndereco).Column("BAIRROCOM");
            Map(x => x.MunicipioEndereco).Column("MUNICCOM");
            Map(x => x.EstadoEndereco).Column("ESTCOM");
            Map(x => x.CodigoUsuario).Column("CODUSUR1");
            Map(x => x.ConsumidorFinal).Column("CONSUMIDORFINAL"); //S
            Map(x => x.Contribuinte).Column("CONTRIBUINTE"); //N
            Map(x => x.CidadeEndereco).Column("CODCIDADEIBGE"); //3828
            Map(x => x.CEP).Column("CEPCOM");
            Map(x => x.Email).Column("EMAIL");
            Map(x => x.CodigoAtividade).Column("CODATV1"); //65
        }
    }
}