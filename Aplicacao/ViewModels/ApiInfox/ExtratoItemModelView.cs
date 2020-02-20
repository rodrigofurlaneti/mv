using System;

namespace Aplicacao.ApiInfox.Models
{
    public class ExtratoItemModelView
    {
        public LojaModelView Loja { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
    }
}