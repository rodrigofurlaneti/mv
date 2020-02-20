using System.Collections.Generic;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Dominio;
using Entidade;

namespace Api.Controllers
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