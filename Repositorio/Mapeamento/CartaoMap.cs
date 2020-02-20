using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CartaoMap : ClassMap<Cartao>
    {
        public CartaoMap()
        {
            Table("Cartao");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Numero).Column("Numero");
            Map(x => x.Cvv).Column("Cvv");
            Map(x => x.Validade).Column("Validade");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.IsEncrypted).Column("IsEncrypted").Default("0");
            Map(x => x.NomeImpresso).Column("NomeImpresso").Nullable();
            Map(x => x.Senha).Column("Senha").Nullable();
            Map(x => x.Token).Column("Token").Nullable();
            Map(x => x.DadosClube).Column("DadosClube").Length(8000).Nullable();
            Map(x => x.MelhorDataComrpa).Column("MelhorDataComrpa").Nullable();

            References(x => x.TipoCartao).Column("TipoCartao").Cascade.None();
            References(x => x.Pessoa).Column("Pessoa").Cascade.None();
        }
    }
}