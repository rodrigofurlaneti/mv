using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class LojaMap : ClassMap<Loja>
    {
        public LojaMap()
        {
            Table("Loja");
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
            Map(x => x.LojaAprovada).Column("LojaAprovada");
            Map(x => x.AceiteContrato).Column("AceiteContrato");
            Map(x => x.DataAceiteContrato).Column("DataAceiteContrato");
            Map(x => x.FaseCadastro).Column("FaseCadastro");
            Map(x => x.Delivery).Column("Delivery");
            Map(x => x.AceiteContratoDelivery).Column("AceiteContratoDelivery");
            Map(x => x.DataAceiteContratoDelivery).Column("DataAceiteContratoDelivery");
            Map(x => x.Classificacao).Column("Classificacao");
            Map(x => x.RaioAtendimento).Column("RaioAtendimento");
            Map(x => x.CodigoInfox).Column("CodigoInfox");

            References(x => x.Endereco).Column("Endereco").Nullable().Cascade.All();

            References(x => x.ProdutoAtivacaoLoja).Column("ProdutoAtivacaoLoja").Cascade.All();

            HasMany(x => x.TerminaisLoja)
                .Table("TerminalCobrancaLoja")
                .KeyColumn("Loja_id")
                .Cascade.All().Inverse();
            
            References(x => x.Proprietario).Column("Proprietario").Nullable().Cascade.All();
            References(x => x.DadosBancario).Column("DadosBancario").Nullable().Cascade.All();

            HasMany(x => x.HorarioFuncionamento).Table("HorarioFuncionamentoLoja")
                .KeyColumn("Loja")
                .Inverse();

            HasMany(x => x.HorarioDelivery)
                .Table("HorarioDeliveryLoja")
                .KeyColumn("Loja")
                .Component(m =>
                {
                    m.Map(x => x.DiaDaSemana).Column("DiaDaSemana");
                    m.Map(x => x.HoraInicio).Column("HoraInicio");
                    m.Map(x => x.HoraFim).Column("HoraFim");
                    m.Map(x => x.Distancia).Column("Distancia");
                    m.Map(x => x.Valor).Column("Valor");
                }).Cascade.All();

            HasMany(x => x.LojaTipoCartaos)
                .Table("LojaTipoCartao")
                .KeyColumn("Loja")
                .Component(m =>
                {
                    m.References(x => x.TipoCartao).Column("TipoCartao").Cascade.None();
                }).Cascade.All();

            HasMany(x => x.LojaTipoLoja)
                .Table("LojaTipoLoja")
                .KeyColumn("Loja")
                .Component(m =>
                {
                    m.References(x => x.TipoLoja).Column("TipoLoja").Cascade.All();
                }).Cascade.All();
        }
    }
}