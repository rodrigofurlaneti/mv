using System;
using Entidade;

namespace Api.Models
{
    public class PessoaModelView
    {
        public PessoaModelView()
        {
            
        }

        public PessoaModelView(Pessoa pessoa)
        {
            if (pessoa == null)
                throw new Exception("A pessoa não foi encontrado.");

            Id = pessoa.Id;
            Nome = pessoa.Nome;
            Sobrenome = pessoa.Sobrenome;
            Sexo = pessoa.Sexo;
            DataNascimento = pessoa.DataNascimento;
            Cpf = pessoa.Cpf;
            Rg = pessoa.Rg;
            Celular = pessoa.Celular;
            Email = pessoa.Email;
        }

        public int Id { get; set; }

        public string Nome { get; set; }
        
        public string Sobrenome { get; set; }
        
        public string Sexo { get; set; }
        
        public DateTime DataNascimento { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }
        
        public string Celular { get; set; }

        public string Email { get; set; }
    }
}