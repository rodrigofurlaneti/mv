using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ApiInfox.Base;
using ApiInfox.Models;
using Dominio;
using Entidade;

namespace ApiInfox.Controllers
{
    [RoutePrefix("api/produto")]
    public class ProdutoController : BaseController<Produto, IProdutoServico>
    {
        [HttpGet]
        [Route("produtosPorDepartamento/{departamentoId}/{lojaId}")]
        public List<Produto> GetProdutosPorDepartamento(int departamentoId, int lojaId, int inicio, int quantidade)
        {
            return Servico.GetProdutosPorDepartamento(departamentoId, lojaId).Skip(inicio).Take(quantidade).ToList();
        }

        [HttpGet]
		[Route("produtosPorSecao/{secaoId}/{lojaId}")]
		public List<Produto> GetProdutosPorSecao(int secaoId, int lojaId, int inicio, int quantidade)
		{
			return Servico.GetProdutosPorSecao(secaoId, lojaId).Skip(inicio).Take(quantidade).ToList();
		}

	    [HttpGet]
	    [Route("produtosPorGrupo/{grupoId}/{lojaId}")]
	    public List<Produto> GetProdutosPorGrupo(int grupoId, int lojaId, int inicio, int quantidade)
	    {
		    return Servico.GetProdutosPorGrupo(grupoId, lojaId).Skip(inicio).Take(quantidade).ToList();
	    }

	    [HttpGet]
	    [Route("produtosPorSubGrupo/{subSubGrupoId}/{lojaId}")]
	    public List<Produto> GetProdutosPorSubGrupo(int subSubGrupoId, int lojaId, int inicio, int quantidade)
	    {
		    return Servico.GetProdutosPorSubGrupo(subSubGrupoId, lojaId).Skip(inicio).Take(quantidade).ToList();
	    }

	    [HttpGet]
	    [Route("produtosPorCategoria/{categoriaId}/{lojaId}")]
	    public List<Produto> GetProdutosPorCategoria(int categoriaId, int lojaId, int inicio, int quantidade)
	    {
		    return Servico.GetProdutosPorCategoria(categoriaId, lojaId).Skip(inicio).Take(quantidade).ToList();
	    }

        [HttpGet]
        [Route("produtoPorCodigoBarras/{codigoBarras}")]
        public ProdutoModelView GetProdutoPorCodigoBarras(long codigoBarras)
        {
            return new ProdutoModelView(Servico.BuscarPor(x => x.CodigoBarras == codigoBarras).FirstOrDefault());
        }
    }
}