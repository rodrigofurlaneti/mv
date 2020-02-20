using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class HorarioFuncionamentoLojaMap : ClassMap<HorarioFuncionamentoLoja>
    {
        public HorarioFuncionamentoLojaMap()
        {
            Table("HorarioFuncionamentoLoja");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DiaDaSemana).Column("DiaDaSemana");
            Map(x => x.HoraInicio).Column("HoraInicio");
            Map(x => x.HoraFim).Column("HoraFim");
            References(x => x.Loja).Column("Loja").Cascade.None();
        }
    }
}