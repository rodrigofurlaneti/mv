using System.Collections.Generic;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/tabloide")]
    public class TabloideController : BaseController<Tabloide, ITabloideServico>
    {
        [HttpGet]
        [Route("tabloideatual")]
        public IEnumerable<TabloideModelView> GetDescontos()
        {
            var tabloides = new List<TabloideModelView>();
            foreach (var tabloide in Servico.Buscar())
                tabloides.Add(new TabloideModelView(tabloide.URL));

            return tabloides;
        }
    }
}