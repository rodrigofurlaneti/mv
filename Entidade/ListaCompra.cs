using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade.Base;

namespace Entidade
{
    public class ListaCompra : BaseEntity
    {
        public ListaCompra()
        {
            Itens = new List<ItemCompra>();
        }

        public virtual IEnumerable<ItemCompra> Itens { get; set; }

        public virtual string Cupom { get; set; }

        public virtual decimal ValorCupom { get; set; }

        private decimal _total { get; set; }
        public virtual decimal Total
        {
            get
            {
                _total = 0;
                if (Itens == null || !Itens.Any())
                    return _total;

                _total = Itens.Sum(x => x.Total) - ValorCupom;
                return _total;
            }
            set { _total = value; }
        }

        private decimal _subtotal { get; set; }
        public virtual decimal SubTotal
        {
            get
            {
                _subtotal = 0;
                if (Itens == null || !Itens.Any())
                    return _subtotal;

                _subtotal = Itens.Sum(x => x.Total);
                return _subtotal;
            }
            set { _subtotal = value; }
        }

        public virtual Usuario Usuario { get; set; }

        [Required]
        public virtual Loja Loja { get; set; }
    }
}