using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Api.Base;
using Dominio;
using Entidade;

namespace Api.Controllers
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

        [HttpGet]
        [Route("DepartamentosPorNome")]
        public List<DepartamentoProduto> GetDepartamentosPorNome(string nome)
        {
            return Servico.BuscarPor(x => x.Nome.Contains(nome)).ToList() ?? new List<DepartamentoProduto>();
        }
    }
}