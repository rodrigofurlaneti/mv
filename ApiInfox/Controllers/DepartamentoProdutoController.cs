using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ApiInfox.Base;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [Authorize]
    [RoutePrefix("api/DepartamentoProduto")]
    public class DepartamentoProdutoController : BaseController<DepartamentoProduto, IDepartamentoProdutoServico>
    {
        [HttpGet]
        [Route("Departamentos")]
        public List<DepartamentoProduto> GetDepartamentos()
        {
            return Servico.Buscar().ToList() ?? new List<DepartamentoProduto>();
        }

        [HttpGet]
        [Route("DepartamentosPorLoja/{loja}")]
        public List<DepartamentoProduto> GetDepartamentosPorLoja(int loja)
        {
            return Servico.BuscarPorLoja(loja).ToList() ?? new List<DepartamentoProduto>();
        }

        [HttpGet]
        [Route("DepartamentosPorCategoria/{categoria}")]
        public List<DepartamentoProduto> GetDepartamentosPorCategoria(int categoria)
        {
            return Servico.BuscarPor(x => x.CategoriaProduto.Id == categoria).ToList() ?? new List<DepartamentoProduto>();
        }
    }
}