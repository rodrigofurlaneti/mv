using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/documento")]
    public class DocumentoController : BaseController<Documento, IDocumentoServico>
    {
        
    }
}