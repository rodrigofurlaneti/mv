using System.Collections.Generic;
using System.Web.Http;
using Api.Base;
using Api.Helpers;
using Api.Models;
using Dominio;
using Entidade;

namespace Api.Controllers
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

            var usuario = LoggedUser.GetLoggedUser();
            if (usuario == null)
            foreach (var tutorial in Servico.BuscarPor(x => !x.Logado))
                tutoriais.Add(new TutorialModelView(tutorial.Id, tutorial.URL));
            else
                foreach (var tutorial in Servico.BuscarPor(x => x.Logado))
                    tutoriais.Add(new TutorialModelView(tutorial.Id, tutorial.URL));

            return tutoriais;
        }
    }
}