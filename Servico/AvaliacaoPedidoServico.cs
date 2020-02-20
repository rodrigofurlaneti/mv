using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IAvaliacaoPedidoServico : IBaseServico<AvaliacaoPedido>
    {
    }

    public class AvaliacaoPedidoServico : BaseServico<AvaliacaoPedido, IAvaliacaoPedidoRepositorio>, IAvaliacaoPedidoServico
    {
        
    }
}