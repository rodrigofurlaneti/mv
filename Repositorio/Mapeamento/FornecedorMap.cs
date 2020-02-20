using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class FornecedorMap : ClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Telefone).Column("Telefone");
            Map(x => x.Celular).Column("Celular");
            Map(x => x.Email).Column("Email");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Cnpj).Column("Cnpj");
            Map(x => x.RazaoSocial).Column("RazaoSocial");
            Map(x => x.InscricaoEstadual).Column("InscricaoEstadual").Nullable(); ;
            Map(x => x.Status).Column("Status");
            Map(x => x.NotaAvaliacao).Column("NotaAvaliacao").Nullable().ReadOnly();
            Map(x => x.LogoUpload).Column("LogoUpload").CustomSqlType("varchar(max)");
            Map(x => x.FornecedorAprovada).Column("FornecedorAprovada");
            Map(x => x.Classificacao).Column("Classificacao");
            References(x => x.Endereco).Column("Endereco").Nullable().Cascade.All();

            HasMany(x => x.HorarioFuncionamento)
                .Table("HorarioFuncionamentoFornecedor")
                .KeyColumn("Fornecedor")
                .Component(m =>
                {
                    m.Map(x => x.DiaDaSemana).Column("DiaDaSemana");
                    m.Map(x => x.HoraInicio).Column("HoraInicio");
                    m.Map(x => x.HoraFim).Column("HoraFim");
                }).Cascade.All();

            HasMany(x => x.HorarioDelivery)
                .Table("HorarioDeliveryFornecedor")
                .KeyColumn("Fornecedor")
                .Component(m =>
                {
                    m.Map(x => x.DiaDaSemana).Column("DiaDaSemana");
                    m.Map(x => x.HoraInicio).Column("HoraInicio");
                    m.Map(x => x.HoraFim).Column("HoraFim");
                    m.Map(x => x.Distancia).Column("Distancia");
                    m.Map(x => x.Valor).Column("Valor");
                }).Cascade.All();
        }
    }
}