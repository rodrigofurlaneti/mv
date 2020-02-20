using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Fornecedor : BaseEntity
    {
        public Fornecedor()
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
        public virtual bool FornecedorAprovada { get; set; }

        public virtual IList<FornecedorProduto> FornecedorProdutos { get; set; }

        public virtual string LogoUpload { get; set; }

        public virtual string Logo()
        {
            return LogoUpload != null
                        ? LogoUpload
                        : "assets/image/logo.png";
        }

        public virtual IList<HorarioFuncionamentoFornecedor> HorarioFuncionamento { get; set; }

        public virtual IList<HorarioDeliveryFornecedor> HorarioDelivery { get; set; }

        public virtual IList<HorarioAgendamentoFornecedor> HorarioAgedamentoFornecedor { get; set; }
    }

    public class HorarioFuncionamentoFornecedor
    {
        public virtual DiaSemana DiaDaSemana { get; set; }

        public virtual string HoraInicio { get; set; }

        public virtual string HoraFim { get; set; }
    }

    public class HorarioDeliveryFornecedor
    {
        public virtual DiaSemana DiaDaSemana { get; set; }

        public virtual string HoraInicio { get; set; }

        public virtual string HoraFim { get; set; }

        public virtual int Distancia { get; set; }

        public virtual decimal Valor { get; set; }
    }

    public class HorarioAgendamentoFornecedor
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
