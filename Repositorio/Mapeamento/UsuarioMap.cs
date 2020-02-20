using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("Usuario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Login).Column("Login");
            Map(x => x.Senha).Column("Senha");
            Map(x => x.ImagemUpload).Column("ImagemUpload");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.PrimeiroLogin).Column("PrimeiroLogin").Not.Nullable().Default("1");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            Map(x => x.Token).Column("Token").Length(255);
            Map(x => x.UltimoAcesso).Column("UltimoAcesso");
            Map(x => x.IsEncrypted).Column("IsEncrypted").Default("0");
            Map(x => x.FacebookId);
            Map(x => x.Perfil).Column("PerfilApp").Nullable();

            References(x => x.Pessoa).Column("Pessoa").Nullable();

            HasMany(x => x.Perfils)
                .Table("UsuarioPerfil")
                .KeyColumn("Usuario")
                .Component(m =>
                {
                    m.References(x => x.Perfil).Column("Perfil_id").Cascade.None();
                });

            HasMany(x => x.ListaUsuarioLoja)
                .Table("UsuarioLoja")
                .KeyColumn("Usuario")
                .Component(m =>
                {
                    m.References(x => x.Loja).Cascade.None();
                });

            HasMany(x => x.ListaUsuarioConvenio)
                .Table("UsuarioConvenio")
                .KeyColumn("Usuario")
                .Component(m =>
                {
                    m.References(x => x.Convenio).Column("Convenio").Cascade.None();
                });

            HasMany(x => x.ListaUsuarioCombo)
                .Table("UsuarioCombo")
                .KeyColumn("Usuario")
                .Component(m =>
                {
                    m.References(x => x.PlanoVenda).Column("PlanoVenda").Cascade.None();
                });
        }
    }
}
