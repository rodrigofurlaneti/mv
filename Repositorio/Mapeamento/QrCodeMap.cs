using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class QrCodeMap : ClassMap<QrCode>
    {
        public QrCodeMap()
        {
            Table("QrCode");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Url).Column("Url");
            Map(x => x.CodigoConfirmacao).Column("CodigoConfirmacao");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}