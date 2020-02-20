using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Table("Pessoa");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.DataNascimento).Column("DataNascimento");
            Map(x => x.Sexo).Column("Sexo").Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.CodClienteInfox).Column("CodClienteInfox").Nullable().Default("12049");

            HasMany(x => x.Documentos);

            HasMany(x => x.Contatos)
                .Table("PessoaContato")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.Contato).Cascade.All();
                }).Cascade.All();

            HasMany(x => x.Cartoes);
            HasMany(x => x.EnderecosEntrega);

            HasMany(x => x.ListaDescontoPessoa)
                .Table("DescontoPessoa")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.ProdutoPreco).Column("ProdutoPreco").Cascade.None();
                    m.References(x => x.Desconto).Column("Desconto").Cascade.None();
                })
                .Inverse()
                .Cascade.All();
        }
    }
}