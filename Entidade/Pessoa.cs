using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Pessoa : BaseEntity
    {
        public Pessoa()
        {
            Documentos = new List<Documento>();
            Contatos = new List<PessoaContato>();
            Cartoes = new List<Cartao>();
            DataNascimento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
        [Required]
        public virtual string Nome { get; set; }
        [Required]
        public virtual string Sobrenome { get; set; }
        
        public virtual string Sexo { get; set; }
        [Required]
        public virtual DateTime DataNascimento { get; set; }
        
        public virtual IList<Documento> Documentos { get; set; }
        public virtual IList<PessoaContato> Contatos { get; set; }
        public virtual IList<Cartao> Cartoes { get; set; }
        public virtual IList<Endereco> EnderecosEntrega { get; set; }

        //Retorna o primeiro email de contatos -- (Não Mapear)
        private string _email { get; set; }
        public virtual string Email
        {
            get
            {
                return !string.IsNullOrEmpty(_email)
                    ? _email
                    : Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Email)?.Contato.Email
                      ?? string.Empty;
            }
            set { _email = value; }
        }

        public virtual string Cpf
        {
            get
            {
                var cpf = Documentos.FirstOrDefault(x => x.Tipo == (int) TipoDocumento.Cpf);
                return cpf?.Numero;
            }
        }

        public virtual string Rg
        {
            get
            {
                var cpf = Documentos.FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Rg);
                return cpf?.Numero;
            }
        }

        public virtual string Cnpj
        {
            get
            {
                var cnpj = Documentos.FirstOrDefault(x => x.Tipo == (int)TipoDocumento.Cnpj);
                return cnpj?.Numero;
            }
        }

        //Retorna o primeiro email de contatos -- (Não Mapear)
        private string _celular { get; set; }
        public virtual string Celular
        {
            get
            {
                return !string.IsNullOrEmpty(_celular)
                    ? _celular
                    : Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Celular)?.Contato.Numero
                      ?? string.Empty;
            }
            set { _celular = value; }
        }

        public virtual IList<DescontoPessoa> ListaDescontoPessoa { get; set; }

        //public virtual int Idade => RetornarIdade(DataNascimento);

        //private static int RetornarIdade(DateTime dob)
        //{
        //    if (dob == SqlDateTime.MinValue || dob == DateTime.MinValue)
        //        return 0;

        //    var age = DateTime.Now.Year - dob.Year;
        //    if (DateTime.Now.DayOfYear < dob.DayOfYear) age = age - 1;
        //    return age;
        //}

        public virtual string CodClienteInfox { get; set; }

        public virtual Endereco EnderecoComercial
        {
            get
            {
                return EnderecosEntrega != null ? EnderecosEntrega.FirstOrDefault(e => e.Tipo == (int)TipoEndereco.Comercial): null;
            }
        }

        public virtual Endereco EnderecoResidencial
        {
            get
            {
                return EnderecosEntrega != null ? EnderecosEntrega.FirstOrDefault(e => e.Tipo == (int)TipoEndereco.Residencial) : null;
            }
        }

        public virtual Convenio Convenio { get; set; }

        public virtual PlanoVenda PlanoVenda { get; set; }
    }
}