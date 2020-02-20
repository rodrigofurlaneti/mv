using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PlanoVendaMap : ClassMap<PlanoVenda>
    {
        public PlanoVendaMap()
        {
            Table("PlanoVenda");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Nome).Column("Nome");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Foto).Column("Foto");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(8).Scale(2);
            Map(x => x.ValorDesconto).Column("ValorDesconto").Length(10).Precision(8).Scale(2);
            Map(x => x.InicioVigencia).Column("InicioVigencia");
            Map(x => x.FimVigencia).Column("FimVigencia");
            Map(x => x.Status).Column("Status").Default("0");

            References(p => p.Convenio).Columns("Convenio").Cascade.None();

            HasMany(x => x.Percentuais).Table("PercentuaisPlanoVenda").KeyColumn("PlanoVenda").Element("Percentual").AsBag();

            HasMany(x => x.Beneficios).Table("BeneficiosPlanoVenda").KeyColumn("PlanoVenda").Element("Beneficio").AsBag();

            HasMany(x => x.Fotos).Table("FotosPlanoVenda").KeyColumn("PlanoVenda").Element("Foto").AsBag();
        }
    }
}