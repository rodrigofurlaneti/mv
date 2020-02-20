using System.Collections.Generic;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/tutorial")]
    public class TutorialController : BaseController<Tutorial, ITutorialServico>
    {
        [HttpGet]
        [Route("tutoriais")]
        public IEnumerable<TutorialModelView> GetTutorial()
        {
            var tutoriais = new List<TutorialModelView>();
            foreach (var tutorial in Servico.Buscar())
                tutoriais.Add(new TutorialModelView(tutorial.URL));

            return tutoriais;
        }
    }
}