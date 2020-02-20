using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class UsuarioViewModel
    {
        public int? Id { get; set; }

        public PessoaViewModel Pessoa { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public IList<UsuarioPerfilViewModel> Perfils { get; set; }

        public string GetImage()
        {
            return AvatarUpload != null && AvatarUpload.Any()
                        ? $"data:image/jpg;base64,{Convert.ToBase64String(AvatarUpload)}"
                        : "../../Content/img/avatars/sunny-big.png";
        }

        public string NomeId => Id + " - " + Pessoa?.Nome;

        public bool PrimeiroLogin { get; set; }

        public bool Ativo { get; set; }

        public byte[] AvatarUpload { get; set; }

        public List<int> ListaPerfilId { get; set; }
    }
}