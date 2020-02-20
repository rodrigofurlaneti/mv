using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Agendamento : BaseEntity
    {
        public Agendamento()
        {
            Data = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        [Required(ErrorMessage = "*")]
        public virtual DateTime Data { get; set; }

        public virtual int CapacidadeDeAtendimento { get; set; }

        public virtual bool Disponivel
        {
            get
            {
                if ((Pedidos != null && Pedidos.Count >= CapacidadeDeAtendimento))
                    return false;

                if ((ItensCompra != null && ItensCompra.Count >= CapacidadeDeAtendimento))
                    return false;

                if (Pedidos != null && ItensCompra != null && Pedidos.Count + ItensCompra.Count >= CapacidadeDeAtendimento)
                    return false;

                return true;
            }
        }

        [Required(ErrorMessage = "*")]
        public virtual Loja Loja { get; set; }

        public virtual IList<Pedido> Pedidos { get; set; }

        public virtual IList<ItemCompra> ItensCompra { get; set; }
    }
}