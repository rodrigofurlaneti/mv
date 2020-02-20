using System.Collections.Generic;
using Entidade.Base;

namespace Entidade
{
    public class PedidoIntegracao : BaseEntity
    {
        private readonly Pedido _pedido;

        public PedidoIntegracao(Pedido pedido)
        {
            _pedido = pedido;
            Itens = new List<ItemCompra>();
            SetData();
        }

        private void SetData()
        {
            PedidoId = _pedido.Id;
            Pessoa = _pedido.Usuario.Pessoa;
            Loja = _pedido.ListaCompra.Loja;
            Total = _pedido.ListaCompra.Total;
            foreach (var item in _pedido.ListaCompra.Itens)
                Itens.Add(item);
            Cartao = _pedido.Cartao;
            Total = _pedido.ListaCompra.Total;
        }

        public int PedidoId { get; set; }
        public Pessoa Pessoa { get; set; }
        public Loja Loja { get; set; }
        public IList<ItemCompra> Itens { get; set; }
        public Cartao Cartao { get; set; }
        public decimal Total { get; set; }
    }
}