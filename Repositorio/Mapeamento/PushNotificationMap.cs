using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PushNotificationMap : ClassMap<PushNotification>
    {
        public PushNotificationMap()
        {
            Table("PushNotification");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Mensagem).Column("Mensagem");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}