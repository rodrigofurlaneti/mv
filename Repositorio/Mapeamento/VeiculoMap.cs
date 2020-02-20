using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class VeiculoMap : ClassMap<Veiculo>
    {
        public VeiculoMap()
        {
            Table("Veiculo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Placa).Column("Placa").Not.Nullable();
            Map(x => x.ModeloId).Column("ModeloId").Not.Nullable();
            Map(x => x.Modelo).Column("Modelo").Not.Nullable();
            Map(x => x.Ano).Column("Ano");
            Map(x => x.TipoVeiculo).Column("TipoVeiculo").Not.Nullable();
            Map(x => x.TipoOutros).Column("TipoOutros");
            Map(x => x.MotoristaDeAplicativo).Column("MotoristaDeAplicativo");
            Map(x => x.Taxista).Column("Taxista");
            References(x => x.Marca).Column("Marca").Cascade.None();
            References(x => x.Proprietario).Column("Proprietario").Cascade.None();

            HasMany(x => x.OutrosProprietarios)
            .Table("OutrosProprietarios")
            .KeyColumn("Usuario").Cascade.None();
        }
    }
}
