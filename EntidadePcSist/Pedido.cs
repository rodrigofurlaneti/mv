using System;
using System.Collections.Generic;
using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class Pedido : IEntityPcSist
    {
        public Pedido()
        {
            Items = new List<ItemCompra>();
        }
        
        public virtual int Importado { get; set; }
        public virtual int CodigoPedidoRCA { get; set;} //PK Pedido
        public virtual int CodigoRCA { get; set; } //PK Pedido
        public virtual string DocumentoCliente { get; set; }
        public virtual DateTime DataAbertura { get; set; }
        public virtual DateTime DataFechamento { get; set; }
        public virtual string CodigoFilial { get; set; }
        public virtual string CodigoCobranca { get; set; }
        public virtual int CodigoPlanoPagamento { get; set; }
        public virtual int CondicaoVenda { get; set; }
        public virtual string Origem { get; set; }
        public virtual List<ItemCompra> Items { get; set; }
        public virtual float ValorDesconto { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}