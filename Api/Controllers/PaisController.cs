using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/pais")]
    public class PaisController : BaseController<Pais, IPaisServico>
    {
        
    }
}