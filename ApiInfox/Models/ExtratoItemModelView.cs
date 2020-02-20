using System;

namespace ApiInfox.Models
{
    public class ExtratoItemModelView
    {
        public LojaModelView Loja { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}