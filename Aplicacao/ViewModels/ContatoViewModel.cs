using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ContatoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoContato Tipo { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string NomeRecado { get; set; }
        public int Ordem { get; set; }
        public bool Blacklist { get; set; }
        public int CodigoMotivo { get; set; }
        public string DescricaoMotivo { get; set; }
        
        //Mapeamento para Importacao Apenas
        public string DDD { get; set; }
        public string Numero { get; set; }
        
        public string Resumo()
        {
            if (!string.IsNullOrEmpty(this.Email))
                return this.Email;

            if (!string.IsNullOrEmpty(Telefone))
                return this.Telefone;

            if (!string.IsNullOrEmpty(Celular))
                return this.Celular;

            return string.Empty;
        }

        public ContatoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ContatoViewModel(Contato contato)
        {
            Id = contato?.Id ?? 0;
            Email = contato?.Email;
            Celular = contato?.Tipo == TipoContato.Celular ? contato.Numero : "";
            Telefone = contato?.Tipo == TipoContato.Residencial || contato?.Tipo == TipoContato.Comercial ? contato.Numero : "";
            Tipo = contato?.Tipo ?? 0;
            DataInsercao = contato?.DataInsercao ?? DateTime.Now;
        }

        public static List<ContatoViewModel> ContatoViewModelList(IList<Contato> contatos)
        {
            var contatosVm = new List<ContatoViewModel>();
            if (contatos == null || contatos.Count <= 0) return contatosVm;

            contatosVm.AddRange(contatos.Select(contato => new ContatoViewModel(contato)));
            return contatosVm;
        }

        public Contato ToEntity() => new Contato
        {
            Id = Id,
            DataInsercao = DateTime.Now,
            Numero = this.RetornarNumero(),
            Tipo = this.Tipo,
            Email = this.Email
        };

        private string RetornarNumero()
        {
            if (this.Tipo == TipoContato.Residencial || this.Tipo == TipoContato.Celular || this.Tipo == TipoContato.Comercial || this.Tipo == 0)
            {
                if (!string.IsNullOrEmpty(this.DDD) || !string.IsNullOrEmpty(this.Numero))
                    return string.Concat(this.DDD, " ", this.Numero).Trim();

                if (!string.IsNullOrEmpty(this.Celular))
                {
                    this.Tipo = TipoContato.Celular;
                    return this.Celular;
                }

                if (!string.IsNullOrEmpty(this.Telefone))
                {
                    this.Tipo = TipoContato.Residencial;
                    return this.Telefone;
                }
            }

            this.Tipo = TipoContato.Email;

            return null;
        }
    }
}