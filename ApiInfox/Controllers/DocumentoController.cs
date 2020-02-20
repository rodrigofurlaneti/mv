using System.Web.Http;
using ApiInfox.Base;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/documento")]
    public class DocumentoController : BaseController<Documento, IDocumentoServico>
    {
        
    }
}