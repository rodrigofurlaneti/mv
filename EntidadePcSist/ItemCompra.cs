using System;
using EntidadePcSist.Base;

namespace EntidadePcSist
{
    public class ItemCompra : IEntityPcSist
    {
        public virtual string Id { get; set; }
        public virtual int CodigoPedigoRCA { get; set; } //FK Pedido
        public virtual int CodigoRCA { get; set; } //FK Pedido
        public virtual int CodigoProduto { get; set; } //FK Produto
        public virtual string DocumentoCliente { get; set; }
        public virtual DateTime DataAberturaPedido { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal PrecoUnidade { get; set; }
        public virtual int NumeroSequencia { get; set; }
        public virtual float PercentualDescontoEDI { get; set; }
        public virtual float PercentualDescontoBoleto { get; set; }
        public virtual Pedido Pedido { get; set; }
        public virtual string CodigoAuxiliar { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            ItemCompra other = obj as ItemCompra;
            if (other == null)
                return false;
            if (CodigoPedigoRCA == other.CodigoPedigoRCA && CodigoProduto == other.CodigoProduto && NumeroSequencia == other.NumeroSequencia)
                return true;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return CodigoPedigoRCA.GetHashCode() ^
                   CodigoProduto.GetHashCode() ^
                   NumeroSequencia.GetHashCode();
        }
    }
}