using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DispositivoMap : ClassMap<Dispositivo>
    {
        public DispositivoMap()
        {
            Table("Dispositivo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Identificador).Not.Nullable();
            Map(x => x.DataInsercao).Not.Nullable();

            References(x => x.Usuario);
        }
    }
}
