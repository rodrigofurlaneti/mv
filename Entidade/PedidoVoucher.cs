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
    public class PedidoVoucher : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual QrCode QrCode { get; set; }
        [JsonConverter(typeof(EnumDescriptionStringConverter))]
        [Required]
        public virtual StatusPedido Status => ListaHistorico?.LastOrDefault()?.StatusPedido ?? StatusPedido.EmAberto;

        public virtual Loja Loja { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        public virtual ProdutoPreco ProdutoPreco { get; set; }

        public virtual decimal ValorVoucher { get; set; }

        public virtual IList<HistoricoPedidoVoucher> ListaHistorico { get; set; }
        
        public virtual void AddListaHistorico(StatusPedido status, Usuario usuario, PedidoVoucher pedido, string descricao = null)
        {
            if (ListaHistorico == null)
                ListaHistorico = new List<HistoricoPedidoVoucher>();

            ListaHistorico.Add(new HistoricoPedidoVoucher
            {
                StatusPedido = status,
                Descricao = descricao ?? status.ToDescription(),
                UsuarioLogado = usuario,
                Pedido = pedido
            });
        }

        public virtual void UpdListaHistorico(StatusPedido status, Usuario usuario, PedidoVoucher pedido, string descricao = null)
        {
            var historico = ListaHistorico
                .Where(lh => lh.Pedido.Id == pedido.Id && lh.Pedido.Usuario.Id == usuario.Id && lh.StatusPedido == status)
                .OrderByDescending(lh => lh.DataInsercao)
                .FirstOrDefault();

            historico.Descricao = descricao ?? historico.Descricao;
        }
    }
}