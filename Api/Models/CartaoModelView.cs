using System;
using Entidade;

namespace Api.Models
{
    public class CartaoModelView
    {
        public CartaoModelView(Cartao cartao)
        {
            if (cartao == null)
                throw new Exception("O cartao não foi encontrado.");

            Id = cartao.Id;
            Numero = cartao.Numero;
            NumeroSemMascara = cartao.NumeroSemMascara;
            Cvv = cartao.Cvv;
            CvvSemMascara = cartao.CvvSemMascara;
            Validade = cartao.Validade;
            IsEncrypted = cartao.IsEncrypted;
            Decrypted = cartao.Decrypted;
            TipoCartao = cartao?.TipoCartao;
            NomeImpresso = string.IsNullOrEmpty(cartao.NomeImpresso) ? cartao.Pessoa?.Nome ?? "" : cartao.NomeImpresso;
            Senha = cartao.Senha;
        }

        public int Id { get; set; }
        public string Numero { get; set; }
        public string NumeroSemMascara { get; set; }
        public string Cvv { get; set; }
        public string CvvSemMascara { get; set; }
        public string Validade { get; set; }

        public bool IsEncrypted { get; set; }
        public bool Decrypted { get; set; }

        public TipoCartao TipoCartao { get; set; }
        public string NomeImpresso { get; set; }
        public string Senha { get; set; }

        public string SaldoDisponivel { get; set; }
        public string LimiteCredito { get; set; }
        public string DiaVencimento { get; set; }
    }
}