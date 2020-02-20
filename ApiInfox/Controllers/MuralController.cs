using System.Collections.Generic;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/mural")]
    public class MuralController : BaseController<Mural, IMuralServico>
    {
        [HttpGet]
        [Route("murais")]
        public IEnumerable<MuralModelView> GetMural()
        {
            var tutoriais = new List<MuralModelView>();
            foreach (var mural in Servico.Buscar())
                tutoriais.Add(new MuralModelView(mural));

            return tutoriais;
        }
    }
}