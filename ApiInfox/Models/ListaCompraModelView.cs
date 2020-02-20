using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade;
using Entidade.Uteis;

namespace ApiInfox.Models
{
    public class ListaCompraModelView
    {
        public ListaCompraModelView(ListaCompra listaCompra)
        {
            if (listaCompra == null)
                throw new Exception("A lista compra não foi encontrado.");

            Cupom = listaCompra.Cupom;
            ValorCupom = listaCompra.ValorCupom;
            Total = listaCompra.Total;
            SubTotal = listaCompra.Total;

            Itens = new List<ItemCompraModelView>();
            foreach (var item in listaCompra.Itens)
                Itens.Add(new ItemCompraModelView(item));

            Loja = new LojaModelView(listaCompra.Loja);

            Id = listaCompra.Id;
        }

        public int Id { get; set; }

        public IList<ItemCompraModelView> Itens { get; set; }

        public string Cupom { get; set; }

        public decimal ValorCupom { get; set; }

        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public LojaModelView Loja { get; set; }
    }
}