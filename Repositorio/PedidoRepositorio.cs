﻿using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoRepositorio : NHibRepository<Pedido>, IPedidoRepositorio
    {
        public PedidoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}