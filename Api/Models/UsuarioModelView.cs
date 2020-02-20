using Entidade;
using System;

namespace Api.Models
{
    public class UsuarioModelView
    {
        public UsuarioModelView(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("O usuario não foi encontrado.");

            if (usuario?.Pessoa != null) Pessoa = new PessoaModelView(usuario.Pessoa);
            Id = usuario.Id;
            Login = usuario.Login;
        }

        public PessoaModelView Pessoa { get; set; }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}