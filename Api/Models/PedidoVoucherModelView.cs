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
    public class PedidoVoucherModelView
    {
        public PedidoVoucherModelView(PedidoVoucher pedido)
        {
            Id = pedido.Id;
            DataInsercao = pedido.DataInsercao;
            Usuario = new UsuarioModelView(pedido.Usuario);
            QrCode = pedido.QrCode;

            ListaHistorico = new List<HistoricoPedidoVoucherModelView>();
            foreach (var historico in pedido.ListaHistorico)
                ListaHistorico.Add(new HistoricoPedidoVoucherModelView(historico));
            
            ValorVoucher = pedido.ValorVoucher;
            if (pedido.Loja != null)
            Loja = new LojaModelView(pedido.Loja);

            if (pedido.Fornecedor != null)
                Fornecedor = new FornecedorModelView(pedido.Fornecedor);

            ProdutoPreco = new ProdutoPrecoModelView(pedido.ProdutoPreco.Produto, pedido.ProdutoPreco);
        }

        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public UsuarioModelView Usuario { get; set; }
        public QrCode QrCode { get; set; }
        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        [Required]
        public StatusPedido Status => ListaHistorico?.LastOrDefault()?.StatusPedido ?? StatusPedido.EmAberto;

        public IList<HistoricoPedidoVoucherModelView> ListaHistorico { get; set; }
        
        public decimal ValorVoucher { get; set; }

        public LojaModelView Loja { get; set; }

        public ProdutoPrecoModelView ProdutoPreco { get; set; }

        public FornecedorModelView Fornecedor { get; set; }
    }
}