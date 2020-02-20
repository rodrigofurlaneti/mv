using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Core.Extensions;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class Cartao : BaseEntity
    {
        private string _numero;
        private string _cvv;

        [Required, StringLength(100)]
        public virtual string Numero
        {
            get
            {
                return _numero;
            }
            set { _numero = value; }
        }

        public virtual string NumeroSemMascara => !string.IsNullOrEmpty(_numero) ? _numero.ExtractNumbers() : _numero;

        [Required, StringLength(3)]
        public virtual string Cvv
        {
            get { return _cvv; }
            set { _cvv = value; }
        }

        public virtual string CvvSemMascara => _cvv;
        
        [Required, StringLength(5)]
        public virtual string Validade { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public virtual bool IsEncrypted { get; set; }

        //Nao Mapear
        public virtual bool Decrypted { get; set; }

        //Infox
        public virtual TipoCartao TipoCartao { get; set; }
        public virtual string NomeImpresso { get; set; }
        public virtual string Senha { get; set; }

        public virtual string Token { get; set; }
        public virtual string DadosClube { get; set; } //TODO: Mudar para uma estrutura Clube x Cartao

        public virtual int MelhorDataComrpa { get; set; }

        public virtual DateTime? DataValidade
        {
            get
            {
                var ptBr = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
                if (Validade == null) return null;
                var nValidade = Validade.ExtractNumbers();
                return nValidade.Length > 4 ? DateTime.ParseExact(nValidade, "MMyyyy", ptBr) : DateTime.ParseExact(nValidade, "MMyy", ptBr);
            }
        }
    }
}