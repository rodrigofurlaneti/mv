using Entidade;
using System;

namespace ApiInfox.Models
{
    public class UsuarioModelView
    {
        //public UsuarioModelView(Usuario usuario)
        //{
        //    if (usuario == null)
        //        throw new Exception("O usuario não foi encontrado.");

        //    Pessoa = new PessoaModelView(usuario.Pessoa);
        //    Login = usuario.Login;
        //}

        public PessoaModelView Pessoa { get; set; }
        public string Login { get; set; }
    }
}