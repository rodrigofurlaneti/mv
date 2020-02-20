using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MuralMap : ClassMap<Mural>
    {
        public MuralMap()
        {
            Table("Mural");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Titulo).Column("Titulo");
            Map(x => x.Descricao).Column("Descricao").Not.Nullable().Length(1000);
            Map(x => x.FotoCapa).Column("FotoCapa").Not.Nullable();
            Map(x => x.Facebook).Column("Facebook").Not.Nullable();
            Map(x => x.DataPublicacao).Column("DataPublicacao");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            HasMany(x => x.Comentarios)
               .Table("ComentarioMural")
               .KeyColumn("Mural")
               .Component(m =>
               {
                   m.References(x => x.Comentario).Column("Comentario").Cascade.All();
               }).Cascade.All();
        }
    }
}
