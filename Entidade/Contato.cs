﻿using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Contato : BaseEntity
    {
        [Required]
        public virtual TipoContato Tipo { get; set; }

        public virtual string Email { get; set; }
        public virtual string Numero { get; set; }
        public virtual string NomeRecado { get; set; }
        public virtual int Ordem { get; set; }
        public virtual Pessoa Pessoa { get; set; }
       
        //Nao Mapear
        public virtual string DDD { get { return Numero != null && Numero.Length > 4 ? Numero.Substring(0, 4).Replace("(", "").Replace(")", "") : string.Empty; } }
    }
}