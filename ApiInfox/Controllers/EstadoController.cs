using System.Web.Http;
using ApiInfox.Base;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/estado")]
    public class EstadoController : BaseController<Estado, IEstadoServico>
    {
        
    }
}