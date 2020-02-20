using System.ComponentModel.DataAnnotations;
using Core.Serializers;
using Entidade.Base;
using Entidade.Uteis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;

namespace Entidade
{
    public class Pedido : BaseEntity
    {
        public virtual ListaCompra ListaCompra { get; set; }
        public virtual Cartao Cartao { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Agendamento Agendamento { get; set; }
        public virtual QrCode QrCode { get; set; }
        public virtual AvaliacaoPedido AvaliacaoPedido { get; set; }
        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        [Required]
        public virtual StatusPedido Status => ListaHistorico?.LastOrDefault()?.StatusPedido ?? StatusPedido.EmAberto;

        public virtual string Motivo { get; set; }
        public virtual int CodigoPcSist { get; set; }

        public virtual string CodEstabelecimentoInfox { get; set; }
        public virtual string NSURedeRXInfox { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual Loja Loja { get; set; }

        public virtual IList<HistoricoPedido> ListaHistorico { get; set; }

        public virtual void AddListaHistorico(StatusPedido status, Usuario usuario, Pedido pedido, string descricao = null, string codigoRetornoTransacao = null)
        {
            if (ListaHistorico == null)
                ListaHistorico = new List<HistoricoPedido>();

            ListaHistorico.Add(new HistoricoPedido
            {
                StatusPedido = status,
                Descricao = descricao ?? status.ToDescription(),
                UsuarioLogado = usuario,
                CodigoRetornoTransacao = codigoRetornoTransacao,
                Pedido = pedido
            });
        }

        public virtual void UpdListaHistorico(StatusPedido status, Usuario usuario, Pedido pedido, string descricao = null, string codigoRetornoTransacao = null)
        {
            var historico = ListaHistorico
                .Where(lh => lh.Pedido.Id == pedido.Id && lh.Pedido.Usuario.Id == usuario.Id && lh.StatusPedido == status)
                .OrderByDescending(lh => lh.DataInsercao)
                .FirstOrDefault();

            historico.Descricao = descricao ?? historico.Descricao;
            historico.CodigoRetornoTransacao = codigoRetornoTransacao ?? historico.CodigoRetornoTransacao;
        }
    }
}