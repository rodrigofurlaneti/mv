using System.Collections.Generic;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/avaliacaoPedido")]
    public class AvaliacaoPedidoController : BaseController<AvaliacaoPedido, IAvaliacaoPedidoServico>
    {
    }
}