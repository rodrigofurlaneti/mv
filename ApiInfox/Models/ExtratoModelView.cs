using System;
using System.Collections.Generic;

namespace ApiInfox.Models
{
    public class ExtratoModelView : ResponseModelView
    {
        public string Cabecalho { get; set; }
        public DateTime DataHoraExtrato { get; set; }
        
        public CartaoModelView Cartao { get; set; }

        public string SaldoDisponivel { get; set; }
        public string LimiteCredito { get; set; }
        public string DiaVencimento { get; set; }
        public List<ExtratoItemModelView> Itens { get; set; }
        public string TotalValorItens { get; set; }
    }
}