using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Entidade.Base;

namespace Entidade
{
    public class Usuario : BaseEntity
    {
        public Usuario()
        {
            UltimoAcesso = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required]
        public virtual Pessoa Pessoa { get; set; }
        [Required, StringLength(50)]
        public virtual string Login { get; set; }

        [Required, StringLength(50)]
        [IgnoreDataMember]
        public virtual string Senha { get; set; }
        [Required]
        public virtual IList<UsuarioPerfil> Perfils { get; set; }

        public virtual byte[] ImagemUpload { get; set; }

        public virtual string GetImage()
        {
            return ImagemUpload != null && ImagemUpload.Any()
                        ? $"data:image/jpg;base64,{Convert.ToBase64String(ImagemUpload)}"
                        : "../../Content/img/avatars/sunny-big.png";
        }

        public virtual string NomeId => Id + " - " + Pessoa?.Nome;

        public virtual bool PrimeiroLogin { get; set; }

        public virtual bool Ativo { get; set; }

        public virtual List<int> ListaPerfilId { get; set; }

        public virtual string Token { get; set; }

        public virtual DateTime UltimoAcesso { get; set; }

        public virtual bool IsEncrypted { get; set; }

        public virtual IList<UsuarioLoja> ListaUsuarioLoja { get; set; }

        public virtual int Perfil { get; set; }

	    public virtual string FacebookId { get; set; }

        public virtual IList<UsuarioConvenio> ListaUsuarioConvenio { get; set; }

        public virtual IList<UsuarioCombo> ListaUsuarioCombo { get; set; }
    }
}