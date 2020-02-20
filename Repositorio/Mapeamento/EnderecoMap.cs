using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EnderecoMap : ClassMap<Endereco>
    {
        public EnderecoMap()
        {
            Table("Endereco");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Cep).Column("Cep").Not.Nullable();
            Map(x => x.Tipo).Column("Tipo").Not.Nullable();
            Map(x => x.Logradouro).Column("Logradouro").Not.Nullable();
            Map(x => x.Numero).Column("Numero").Not.Nullable();
            Map(x => x.Complemento).Column("Complemento");
            Map(x => x.Bairro).Column("Bairro");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Latitude).Column("Latitude").Nullable();
            Map(x => x.Longitude).Column("Longitude").Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
                        
            References(x => x.Cidade).Not.LazyLoad().Cascade.SaveUpdate();

            References(x => x.Pessoa).Cascade.None();
        }
    }
}