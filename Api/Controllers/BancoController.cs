using System.Collections.Generic;
using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/banco")]
    public class BancoController : BaseController<Banco, IBancoServico>
    {
        [HttpGet]
        [Route("bancos")]
        public IEnumerable<Banco> GetBanco()
        {
            return Servico.Buscar();
        }
    }
}