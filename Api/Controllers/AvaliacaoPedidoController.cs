using System.Collections.Generic;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/avaliacaoPedido")]
    public class AvaliacaoPedidoController : BaseController<AvaliacaoPedido, IAvaliacaoPedidoServico>
    {
    }
}