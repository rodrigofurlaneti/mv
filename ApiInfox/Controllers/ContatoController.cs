using System;
using System.Web;
using System.Web.Http;
using ApiInfox.Base;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/contato")]
    public class ContatoController : BaseController<Contato, IContatoServico>
    {
		[HttpPost]
		[Route("")]
		public IHttpActionResult Post([FromBody] Contato entity)
		{
			Servico.Salvar(entity);
			return Created(new Uri(VirtualPathUtility.AppendTrailingSlash(Request.RequestUri.ToString()) + entity.Id), entity.Id);
		}
	}
}