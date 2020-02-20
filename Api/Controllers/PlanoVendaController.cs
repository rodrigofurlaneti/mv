using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/planoVenda")]
    public class PlanoVendaController : BaseController<PlanoVenda, IPlanoVendaServico>
    {
        
    }
}