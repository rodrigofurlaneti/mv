using System.Collections.Generic;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/banner")]
    public class BannerController : BaseController<Banner, IBannerServico>
    {
        [HttpGet]
        [Route("home")]
        public IEnumerable<BannerModelView> GetBanner()
        {
            var tutoriais = new List<BannerModelView>();
            foreach (var tutorial in Servico.BuscarPor(x => x.TipoBanner == Entidade.Uteis.TipoBanner.Home))
                tutoriais.Add(new BannerModelView(tutorial.URL));

            return tutoriais;
        }
    }
}