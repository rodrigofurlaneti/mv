using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PessoaJuridicaMap : ClassMap<PessoaJuridica>
    {
        public PessoaJuridicaMap()
        {
            Table("PessoaJuridica");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Nome").Not.Nullable();
            Map(x => x.CNPJ).Column("CNPJ").Not.Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.InscricaoEstadual).Column("InscricaoEstadual");
            Map(x => x.RazaoSocial).Column("RazaoSocial");

            References(x => x.Endereco).Column("Endereco").Cascade.All();

            HasMany(x => x.Contatos)
                .Table("PessoaJuridicaContato")
                .KeyColumn("PessoaJuridica")
                .Component(m =>
                {
                    m.References(x => x.Contato).Cascade.All();
                }).Cascade.All();
        }
    }
}
