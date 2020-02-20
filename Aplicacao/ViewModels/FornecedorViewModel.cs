using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class FornecedorViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string RazaoSocial { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public List<ContatoViewModel> Contatos { get; set; }

        public FornecedorViewModel()
        {
            this.Endereco = new EnderecoViewModel();
            this.Contatos = new List<ContatoViewModel>();
        }

        public FornecedorViewModel(Fornecedor entidade)
        {
            this.Id = entidade?.Id ?? 0;
            this.DataInsercao = entidade?.DataInsercao ?? DateTime.Now;
            this.Descricao = entidade?.Descricao;
            this.InscricaoEstadual = entidade?.InscricaoEstadual;
            this.RazaoSocial = entidade?.RazaoSocial;
            this.CNPJ = entidade?.Cnpj;
            this.Endereco = new EnderecoViewModel(entidade?.Endereco);
            //this.Contatos = ContatoViewModel.ContatoViewModelList(entidade?.Contatos.Select(x => x.Contato).ToList() ?? new List<Contato>());
        }

        public Fornecedor ToEntity() => new Fornecedor()
        {
            Id = this.Id,
            Cnpj = this.CNPJ,
            //Contatos = this?.Contatos?.Select(x => new FornecedorContato() { Contato = x.ToEntity() }).ToList(),
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Descricao = this.Descricao,
            Endereco = this?.Endereco.ToEntity(),
            RazaoSocial = this?.RazaoSocial,
            InscricaoEstadual = this?.InscricaoEstadual
        };
    }
}
