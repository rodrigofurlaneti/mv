using System.Web.Http;
using ApiInfox.Base;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/pais")]
    public class PaisController : BaseController<Pais, IPaisServico>
    {
        
    }
}