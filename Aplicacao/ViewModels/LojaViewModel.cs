using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class LojaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string RazaoSocial { get; set; }
        public int CodigoGrupo { get; set; }
        public int EnderecoId { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public LojaViewModel()
        {
            this.Endereco = new EnderecoViewModel();
            DataInsercao = DateTime.Now;
        }

        public LojaViewModel(Loja loja)
        {
            this.Id = loja?.Id ?? 0;
            this.DataInsercao = loja?.DataInsercao ?? DateTime.Now;
            this.Descricao = loja?.Descricao;
            this.InscricaoEstadual = loja?.InscricaoEstadual;
            this.RazaoSocial = loja?.RazaoSocial;
            this.CNPJ = loja?.Cnpj;
            this.Endereco = new EnderecoViewModel(loja?.Endereco);
        }

        public Loja ToEntity() => new Loja()
        {
            Id = this.Id,
            Cnpj = this.CNPJ,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Descricao = this.Descricao,
            Endereco = this?.Endereco.ToEntity(),
            RazaoSocial = this?.RazaoSocial,
            InscricaoEstadual = this?.InscricaoEstadual,
        };
    }
}