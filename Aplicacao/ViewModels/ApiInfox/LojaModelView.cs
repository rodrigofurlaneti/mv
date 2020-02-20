using Entidade;

namespace Aplicacao.ApiInfox.Models
{
    public class LojaModelView
    {
        public LojaModelView(Loja loja)
        {
            CarregarDadosBasicos(loja);
        }
        
        private void CarregarDadosBasicos(Loja loja)
        {
            Id = loja.Id;
            Descricao = loja.Descricao;
            RazaoSocial = loja.RazaoSocial;
            InscricaoEstadual = loja.InscricaoEstadual;
            Cnpj = loja.Cnpj;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string InscricaoEstadual { get; set; }
        public double Distancia { get; set; }
    }
}