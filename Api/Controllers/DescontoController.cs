using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Api.Base;
using Api.Models;
using Core.Exceptions;
using Dominio;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/desconto")]
    public class DescontoController : BaseController<DescontoGlobal, IDescontoGlobalServico>
    {
        [HttpGet]
        [Route("todosdescontos")]
        public IEnumerable<DescontoGlobalModelView> GetDescontos()
        {
            var descontos = new List<DescontoGlobalModelView>();
            foreach (var desconto in Servico.Buscar())
                descontos.Add(new DescontoGlobalModelView(desconto.Desconto, desconto.ProdutoPreco));

            return descontos;
        }
    }
}