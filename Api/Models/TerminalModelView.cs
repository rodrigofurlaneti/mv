using Core.Extensions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class TerminalModelView
    {
        public LojaModelView Loja { get; set; }
        public UsuarioModelView Usuario { get; set; }
        public String SerialNumber { get; set; }

        public static TerminalModelView FromModel(TerminalCobrancaLoja model)
        {
            var retorno = new TerminalModelView();
            if (model?.Loja != null) retorno.Loja = new LojaModelView(model?.Loja);
            if (model?.Usuario != null) retorno.Usuario = new UsuarioModelView(model?.Usuario);
            retorno.Usuario.Senha = model?.Usuario?.IsEncrypted ?? false ? Crypt.DeCrypt(model?.Usuario?.Senha ?? "") : model?.Usuario.Senha;
            retorno.SerialNumber = model?.Terminal?.NumeroSerial;

            return retorno;
        }
    }
}