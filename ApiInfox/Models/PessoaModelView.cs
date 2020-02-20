using System;
using Entidade;

namespace ApiInfox.Models
{
    public class PessoaModelView
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
    }
}