using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/estado")]
    public class EstadoController : BaseController<Estado, IEstadoServico>
    {
        
    }
}