using Entidade;
using System;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class DocumentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoDocumento Tipo { get; set; }
        public string Numero { get; set; }
        public string OrgaoExpedidor { get; set; }
        public DateTime? DataExpedicao { get; set; }

        public DocumentoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public DocumentoViewModel(Documento documento)
        {
            Id = documento?.Id ?? 0;
            DataInsercao = documento?.DataInsercao ?? DateTime.Now;
            //Tipo = documento?.Tipo ?? TipoDocumento.Cpf;
            Numero = documento?.Numero;
            OrgaoExpedidor = documento?.OrgaoExpedidor;
            DataExpedicao = documento?.DataExpedicao;
        }

        public Documento ToEntity() => new Documento
        {
            Id = Id,
            DataInsercao = DataInsercao,
            //Tipo = Tipo,
            Numero = Numero,
            OrgaoExpedidor = OrgaoExpedidor,
            DataExpedicao = DataExpedicao
        };
    }
}