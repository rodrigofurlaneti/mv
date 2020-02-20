using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class EnderecoViewModel
    {
        public string Cep { get; set; }
        public CidadeViewModel Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public DateTime DataInsercao { get; set; }
        public int Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }        
        public string Resumo => $"{Logradouro}, {Numero} {(!string.IsNullOrWhiteSpace(Complemento) ? " - " + Complemento : string.Empty)} - {Cep} {(!string.IsNullOrWhiteSpace(Cidade.Descricao) ? " - " + Cidade.Descricao : string.Empty)}";

        public EnderecoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public EnderecoViewModel(Endereco endereco)
        {
            Id = endereco?.Id ?? 0;            
            Bairro = endereco?.Bairro;
            Cep = endereco?.Cep;
            Cidade = new CidadeViewModel(endereco?.Cidade);
            Complemento = endereco?.Complemento;
            Logradouro = endereco?.Logradouro;
            Numero = endereco?.Numero;
            DataInsercao = endereco?.DataInsercao ?? DateTime.Now;
            Longitude = endereco?.Longitude;
            Latitude = endereco?.Latitude;
        }

        public Endereco ToEntity() => new Endereco
        {
            Bairro = this.Bairro,
            Cep = this.Cep,
            Cidade = new Cidade() { Descricao = this?.Cidade?.Descricao },
            Complemento = this.Complemento,
            DataInsercao = this.DataInsercao,
            Logradouro = this.Logradouro,
            Id = this.Id,
            Numero = this.Numero,
            Longitude = this.Longitude,
            Latitude = this.Latitude
        };
    }
}