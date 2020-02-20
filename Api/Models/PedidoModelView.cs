using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Serializers;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;

namespace Api.Models
{
    public class PedidoModelView
    {
        public PedidoModelView()
        {

        }

        public PedidoModelView(Pedido pedido)
        {
            Id = pedido.Id;
            DataInsercao = pedido.DataInsercao;
            if (pedido.ListaCompra != null)
                ListaCompra = new ListaCompraModelView(pedido.ListaCompra);
            Cartao = new CartaoModelView(pedido.Cartao);
            Endereco = pedido.Endereco;
            Usuario = new UsuarioModelView(pedido.Usuario);
            Agendamento = pedido.Agendamento != null ? new AgendamentoModelView(pedido.Agendamento) : null;
            QrCode = pedido.QrCode;

            ListaHistorico = new List<HistoricoPedidoModelView>();
            foreach (var historico in pedido.ListaHistorico)
                ListaHistorico.Add(new HistoricoPedidoModelView(historico));

            AvaliacaoPedido = pedido.AvaliacaoPedido ?? new AvaliacaoPedido();

            Valor = pedido.Valor;
            if (pedido.Loja != null)
                Loja = new LojaModelView(pedido.Loja);
        }

        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ListaCompraModelView ListaCompra { get; set; }
        public CartaoModelView Cartao { get; set; }
        public Endereco Endereco { get; set; }
        public UsuarioModelView Usuario { get; set; }
        public AgendamentoModelView Agendamento { get; set; }
        public QrCode QrCode { get; set; }
        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        [Required]
        public StatusPedido Status => ListaHistorico?.LastOrDefault()?.StatusPedido ?? StatusPedido.EmAberto;

        public IList<HistoricoPedidoModelView> ListaHistorico { get; set; }

        public AvaliacaoPedido AvaliacaoPedido { get; set; }

        public decimal Valor { get; set; }

        public LojaModelView Loja { get; set; }
    }
}