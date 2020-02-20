using System;
using System.Collections.Generic;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Loja : BaseEntity
    {
        public Loja()
        {
        }

        public virtual string Descricao { get; set; }
        public virtual string Cnpj { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual bool Status { get; set; }
        public virtual string Classificacao { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Email { get; set; }
        public virtual int NotaAvaliacao { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual bool LojaAprovada { get; set; }

        public virtual string CodigoInfox { get; set; }

        public virtual IList<LojaProduto> LojaProdutos { get; set; }

        public virtual IList<TerminalCobrancaLoja> TerminaisLoja { get; set; }

        public virtual string LogoUpload { get; set; }

        public virtual string Logo()
        {
            return LogoUpload != null
                        ? LogoUpload
                        : "assets/image/logo.png";
        }

        public virtual IList<HorarioFuncionamentoLoja> HorarioFuncionamento { get; set; }

        public virtual IList<HorarioDeliveryLoja> HorarioDelivery { get; set; }

        public virtual IList<HorarioAgendamentoLoja> HorarioAgedamentoLoja { get; set; }

        public virtual Pessoa Proprietario { get; set; }

        public virtual bool AceiteContrato { get; set; }

        public virtual DateTime DataAceiteContrato { get; set; }

        public virtual int FaseCadastro { get; set; }

        public virtual bool Delivery { get; set; }

        public virtual bool AceiteContratoDelivery { get; set; }

        public virtual DateTime DataAceiteContratoDelivery { get; set; }

        public virtual IList<LojaTipoCartao> LojaTipoCartaos { get; set; }

        public virtual IList<LojaTipoLoja> LojaTipoLoja { get; set; }
        
        public virtual string RaioAtendimento { get; set; }

        public virtual DadosBancario DadosBancario { get; set; }

        public virtual string FotoFachada { get; set; }

        public virtual ProdutoAtivacaoLoja ProdutoAtivacaoLoja { get; set; }
    }

    public class HorarioFuncionamentoLoja : IEntity
    {
        public virtual DiaSemana DiaDaSemana { get; set; }

        public virtual string HoraInicio { get; set; }

        public virtual string HoraFim { get; set; }

        public virtual Loja Loja { get; set; }

        public virtual int Id { get; set; }

        //Somente para controle de tela:
        public virtual string Dia { get { return DiaDaSemana.ToString(); } }

        public virtual string Status { get { return string.IsNullOrEmpty(HoraFim) ? string.Empty :
                    (int)DateTime.Now.DayOfWeek != (int)DiaDaSemana ? string.Empty :
                    (int)DateTime.Now.DayOfWeek == (int)DiaDaSemana && DateTime.Now.Hour <= Int32.Parse(HoraFim.Substring(0,2)) ? "Aberto" : "Fechado"; } }
    }

    public class HorarioDeliveryLoja
    {
        public virtual DiaSemana DiaDaSemana { get; set; }

        public virtual string HoraInicio { get; set; }

        public virtual string HoraFim { get; set; }

        public virtual int Distancia { get; set; }

        public virtual decimal Valor { get; set; }
    }

    public class HorarioAgendamentoLoja
    {
        public DiaSemana DiaDaSemana { get; set; }

        public string HoraInicio { get; set; }

        public string Intervalo { get; set; }

        public int CapacidadeAtendimento { get; set; }

        public DateTime DataInicio { get; set; }

        public bool AplicarNoMes { get; set; }

        public bool AplicarNoAno { get; set; }
    }
}
