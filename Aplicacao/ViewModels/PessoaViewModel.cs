using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class PessoaViewModel
    {
        public PessoaViewModel()
        {
            Endereco = new EnderecoViewModel();
            Contatos = new List<ContatoViewModel>();
            Lojas = new List<int>();
        }

        public PessoaViewModel(Pessoa pessoa)
        {
            Id = pessoa.Id;
            DataInsercao = pessoa.DataInsercao;
            Nome = pessoa.Nome;
            Sexo = pessoa.Sexo;
            DataNascimento = pessoa.DataNascimento.ToString("dd/MM/yyyy", new CultureInfo("pt-BR"));
            //IdDocumentoCpf = pessoa.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Id ?? 0;
            //Cpf = pessoa.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Numero ?? string.Empty;
            //IdDocumentoRg = pessoa.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Rg)?.Id ?? 0;
            //Rg = pessoa.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Rg)?.Numero ?? string.Empty;
            //OrgaoExpedidorRg = pessoa.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Rg)?.OrgaoExpedidor ?? string.Empty;
            //DataExpedicaoRg = pessoa.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Rg)?.DataExpedicao;
            Documentos = AutoMapper.Mapper.Map<List<Documento>, List<DocumentoViewModel>>(pessoa.Documentos?.ToList() ?? new List<Documento>());
            //Endereco = new EnderecoViewModel(pessoa.Endereco);
            //Contatos = pessoa.Contatos?.Select(x => new ContatoViewModel(x.Contato))?.ToList();
            //Lojas = pessoa.Lojas.Select(x => x.Loja.Id).ToList();
        }

        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public string Nome { get; set; }
        public string Sexo { get; set; }

        public int IdDocumentoCpf { get; set; }
        public string Cpf { get; set; }
        public int IdDocumentoRg { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidorRg { get; set; }
        public DateTime? DataExpedicaoRg { get; set; }

        public string DataNascimento { get; set; }

        public EnderecoViewModel Endereco { get; set; }
        public List<ContatoViewModel> Contatos { get; set; }
        public List<DocumentoViewModel> Documentos { get; set; }
        public List<int> Lojas { get; set; }
        public string ContatoEmail => Contatos.FirstOrDefault(x => !string.IsNullOrEmpty(x.Email))?.Email;
        
        public Pessoa ToEntity() => new Pessoa
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao,
            Nome = this.Nome,
            Sexo = this.Sexo,
            DataNascimento = string.IsNullOrEmpty(DataNascimento) || DataNascimento == "__/__/____"
                               ? DateTime.MinValue
                               : Convert.ToDateTime(DataNascimento, new CultureInfo("pt-BR")),
            //EnderecosEntrega = this?.Endereco?.ToEntity(),

            //Contatos = this?.Contatos?.Select(x => new PessoaContato { Contato = x.ToEntity() }).ToList(),
            Documentos = RecoverDocument(),
            //Lojas = Lojas.Select(x => new PessoaLoja { Loja = new Empresa { Id = x } }).ToList(),
        };

        public string GetDocumentoCpf()
        {
            return Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Numero ?? string.Empty;
        }
        public string GetContatoResidencial()
        {
            return Contatos?.FirstOrDefault(x => x.Tipo == TipoContato.Residencial)?.Telefone ?? string.Empty;
        }
        public string GetContatoCelular()
        {
            return Contatos?.FirstOrDefault(x => x.Tipo == TipoContato.Celular)?.Celular ?? string.Empty;
        }

        private List<Documento> RecoverDocument()
        {
            var documents = new List<Documento>();
            if (!string.IsNullOrEmpty(Cpf))
                documents.Add(new Documento
                {
                    Id = IdDocumentoCpf,
                    //Tipo = TipoDocumento.Cpf,
                    Numero = Cpf
                });
            if (!string.IsNullOrEmpty(Rg))
                documents.Add(new Documento
                {
                    Id = IdDocumentoRg,
                    //Tipo = TipoDocumento.Rg,
                    Numero = Rg,
                    DataExpedicao = DataExpedicaoRg,
                    OrgaoExpedidor = OrgaoExpedidorRg
                });

            return documents;
        }
    }

    public class PessoaFiltroViewModel
    {
        public int IdadeDe { get; set; }
        public int IdadeAte { get; set; }
        public Sexo Sexo { get; set; }
        public List<int> Cidade { get; set; }
        public List<int> Estado { get; set; }
    }

    public class PessoaRetornoFiltroViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Idade => RetornarIdade(DataNascimento);
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Sexo { get; set; }
        public bool Selected { get; set; }

        public PessoaRetornoFiltroViewModel()
        {
        }

        public PessoaRetornoFiltroViewModel(Pessoa pessoa, bool selected = false)
        {
            Id = pessoa?.Id ?? 0;
            Nome = pessoa?.Nome;
            DataNascimento = pessoa?.DataNascimento ?? DateTime.Now;
            Cidade = pessoa?.EnderecosEntrega?.FirstOrDefault()?.Cidade?.Descricao;
            Estado = pessoa?.EnderecosEntrega?.FirstOrDefault()?.Cidade?.Estado?.Descricao;
            Sexo = pessoa?.Sexo;
            Selected = selected;
        }

        private static int RetornarIdade(DateTime dob)
        {
            var age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear) age = age - 1;
            return age;
        }
    }
}