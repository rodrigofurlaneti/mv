using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
	[Authorize]
	[RoutePrefix("api/dispositivo")]
    public class DispositivoController : BaseController<Dispositivo, IDispositivoServico>
    {
		[HttpPost]
		[Route("adicionar")]
		public void Adicionar([FromBody]DispositivoModel dispositivo)
        {
			Servico.Salvar(new Dispositivo 
				{ 
					Usuario = new Usuario { Id = dispositivo.UsuarioId }, 
					Identificador = dispositivo.Identificador
				});
        }
    }
}