using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ComentarioMap : ClassMap<Comentario>
    {
        public ComentarioMap()
        {
            Table("Comentario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Titulo).Column("Titulo");
            Map(x => x.Descricao).Column("Descricao").Not.Nullable().Length(500);
            Map(x => x.Estrelas).Column("Estrelas");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.Usuario).Column("Usuario").Not.Nullable();
        }
    }
}
