using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class CidadeViewModel
    {
        public CidadeViewModel()
        {
        }

        public CidadeViewModel(Cidade cidade)
        {
            Id = cidade?.Id ?? 0;
            DataInsercao = cidade?.DataInsercao ?? DateTime.Now;
            Descricao = cidade?.Descricao;
            Estado = new EstadoViewModel(cidade?.Estado);
        }

        public int Id { get; private set; }
        public DateTime DataInsercao { get; }
        
        public EstadoViewModel Estado { get; set; }
        public string Descricao { get; set; }

        public override string ToString()
        {
            return $"{Descricao}{(Estado != null && !string.IsNullOrEmpty(Estado.Sigla) ? $"/{Estado.Sigla}" : string.Empty)}";
        }
    }
}
